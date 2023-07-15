using System.Linq;
using Game;
using Grid;
using UnityEngine;

namespace Pieces
{
    public class Ship : Piece
    {
        private Vector2Int _pos1;
        private Vector2Int _pos2;
        private Vector2Int _targetPos1;
        private Vector2Int _targetPos2;
        [SerializeField] private int length;
        
        public void Init(Board grid, Vector2Int pos1, Vector2Int pos2, Controller gameController)
        {
            _pos1 = pos1;
            _pos2 = pos2;
            _targetPos1 = new Vector2Int(-1, -1);
            _targetPos2 = new Vector2Int(-1, -1);
            Grid = grid;
            GameController = gameController;
        }
        // Gets Executed in every Ship on Every Frame
        public void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            var ray = Camera!.ScreenPointToRay(Input.mousePosition); // Send A ray to Mouse position
            if (!Physics.Raycast(ray, out var hit)) return; // If hit -> Continue

            var ship = hit.transform.gameObject.GetComponent<Ship>();
            if (ship && ship.isViking == GameController.IsVikingsMove()) // if pressed on soldier which is turn
            {
                Unselect(); // Unselect every Ship
                ship.Select(); // Select the new Ship
                Debug.Log("Selected");
                return;
            }
            
            // If didnt hit a ship
            var gridPos = Grid.GetGridIndexFromMousePos();
            
            // Check if Position is valid
            if (!Grid.IsInBounds(gridPos) ||
                !Grid.GetGrid(gridPos).IsWater() || 
                Grid.GetGrid(gridPos).IsOccupied()
               ) return;
            
            // Try to setup targetPositions
            if (!Selected) return;
            if (!SetupPositions(gridPos)) return;

            // Try to move
            if (Selected && !IsMoving) MoveTo(_targetPos1, _targetPos2);
            _targetPos1 = new Vector2Int(-1, -1);
            _targetPos2 = new Vector2Int(-1, -1);
            
            Debug.Log("Reset TargetPositions");
            

        }
        private bool SetupPositions(Vector2Int gridPos)
        {
            if (!Grid.IsInBounds(_targetPos1))
            {
                _targetPos1 = gridPos;
                return false;
            }
            if (!Grid.IsInBounds(_targetPos2) && gridPos != _targetPos1)
            {
                _targetPos2 = gridPos;
                return true;
            }

            return false;
        }
        
        private bool MoveTo(Vector2Int pos1, Vector2Int pos2)
        {
            // If: on the same row or column AND distance = length AND all squares are not Occupied => continue 
            if ((pos1.x != pos2.x && pos1.y != pos2.y) ||
                (Mathf.Abs(pos1.x - pos2.x) != length && Mathf.Abs(pos1.y - pos2.y) != length) || 
                Grid.GetSquaresBetween(pos1, pos2).Any(x => x.IsOccupied())) return false;
            
            Unselect();
            GameController.SwitchTurn();
            // Update Position
            Grid.GetSquaresBetween(pos1, pos2).ForEach(x => x.Occupy(this));
            Grid.GetSquaresBetween(pos1, pos2).ForEach(x => x.SetShipType(isViking));
            Grid.GetSquaresBetween(GetPos1(), GetPos2()).ForEach(x => x.UnOccupy());
            Grid.GetSquaresBetween(GetPos1(), GetPos2()).ForEach(x => x.ResetShipType());
            MovementAnim();

            return true;
        }

        private Vector2Int GetPos1()
        {
            return _pos1;
        }

        private Vector2Int GetPos2()
        {
            return _pos2;
        }

        private void MovementAnim()
        {
            //Move and Rotate to pos1 and pos2
            var pos1 = Grid.GetGrid(_targetPos1).GetWorldPos();
            var pos2 = Grid.GetGrid(_targetPos2).GetWorldPos();
            // Rotate towards pos 1 and pos 2
            var rotation = (_targetPos1.x == _targetPos2.x) switch
            {
                true => new Vector3(0, 90, 0),
                _ => new Vector3(0, 0, 0)
            };
            var pos = (pos1 + pos2) / 2;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), 1);
            transform.position = pos;
            _pos1 = _targetPos1;
            _pos2 = _targetPos2;

        }
        
        public override void Capture()
        {
            // Ship Cant be captured
        }
    }
}
