using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public abstract class Soldier : Piece
    {
        
        [SerializeField] private AnimationCurve animCurve;
        private Vector3 _lerpedValue;
        private const float MovementDuration = 1;
        
        // Gets Executed in Every Soldier every Frame
        public virtual void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return; // Wait for Left-Mouse Down
            var ray = Camera!.ScreenPointToRay(Input.mousePosition); // Send A ray to Mouse position
            if (!Physics.Raycast(ray, out var hit)) return; // If hit -> Continue
         
            var piece = hit.transform.gameObject.GetComponent<Soldier>(); // Get this Script
            if (piece && piece.isViking == GameController.IsVikingsMove()) // if pressed on soldier which is turn
            {
                Unselect(); // Unselect this Soldier (Unselect Every Soldier)
                piece.Select(); // Select the new Soldier
            }
            else if (Selected && !IsMoving)
            {
                MoveTo(Grid.GetGridIndexFromWorldPos(hit.point));
            } // Else move if this is the Selected Piece
        
        }

        

        protected abstract bool MoveTo(Vector2Int gridIndex);

        protected IEnumerator MovementAnim(Vector3 start, Vector3 end)
        {
            SetIsMoving(true); // Cant be moved again while moving
            float timeElapsed = 0;
            
            while (timeElapsed < MovementDuration)
            {
                var t = timeElapsed / MovementDuration;

                t = animCurve.Evaluate(t);
                _lerpedValue = Vector3.Lerp(start, end, t);
                transform.position = _lerpedValue;
                timeElapsed += Time.deltaTime;
            
                yield return null;
            }

            _lerpedValue = end;
            SetIsMoving(false); // Can be moved again
        }

        public override void Capture()
        {
            Grid.GetGrid(GridIndex).UnOccupy();
            Destroy(gameObject);   
        }
    }
}
