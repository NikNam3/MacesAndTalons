using System;
using UnityEngine;

namespace Pieces.ChildClasses
{
    public class Traitor : Soldier
    {
        private bool _isNeutral = true;
        private bool _hasBeenRevealed = false;

        public override void Update()
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
            else if (piece && piece.isViking != GameController.IsVikingsMove())
            {
                if (_isNeutral ||
                    isViking != GameController.IsVikingsMove() ||
                    !Selected ||
                    _hasBeenRevealed || 
                    GameController.GetAccomplice().isViking != GameController.GetTraitor().isViking
                    ) return; // If this piece is neutral or
                              // its not this pieces turn or its not selected or
                              // it has already been revealed or 
                              // the same side doesnt own both accomplice and traitor
                              
                _hasBeenRevealed = true;
                MoveTo(piece.GetGridIndex()); // Replace the Opposing Piece
                piece.Capture(); // Removing the Opposing Piece
            }
            else if (Selected && !IsMoving)
            {
                MoveTo(Grid.GetGridIndexFromWorldPos(hit.point));
            } // Else move if this is the Selected Piece
        }


        protected override bool MoveTo(Vector2Int gridIndex)
        {
            Unselect();
            GameController.SwitchTurn((GridIndex, gridIndex, this));
            if (!GameController.GetAccomplice().HasBeenRevealed())
            {
                GameController.SwitchTurn();
            } // Switch Turn back to place the Accomplice
        
            // Update Position
            Grid.GetGrid(GridIndex).UnOccupy();
            Grid.GetGrid(gridIndex).Occupy(this);
            // Movement Anim
            StartCoroutine(MovementAnim(Grid.GetGrid(GridIndex).GetWorldPos(), Grid.GetGrid(gridIndex).GetWorldPos()));
            GridIndex = gridIndex;
            return true;
        }

        public override void Capture()
        {
            if (_isNeutral)
            {
                var capturedBySide = GameController.GetLastMove().Item3.IsViking();
                _isNeutral = false;
                isViking = capturedBySide;
                
                // Capture pieces around
                GameController.GetLastMove().Item3.Capture();
                
                var opposingPos = (GridIndex - GameController.GetLastMove().Item2)*-1 + GridIndex;
                Grid.GetGrid(opposingPos).GetPiece().Capture();
            }
            else
            {
                Grid.GetGrid(GridIndex).UnOccupy();
                Destroy(gameObject);   
            }
        }
        
        public bool HasBeenRevealed()
        {
            return _hasBeenRevealed;
        }
    }
}
