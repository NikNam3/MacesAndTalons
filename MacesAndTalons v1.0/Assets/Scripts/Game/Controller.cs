using System.Linq;
using Grid;
using Pieces;
using Pieces.ChildClasses;
using UnityEngine;

namespace Game
{
    public class Controller : MonoBehaviour
    {
        private bool _isVikingsMove;

        private Traitor _traitor;
        private Accomplice _accomplice;
        
        [SerializeField] private Board grid;

        // Last Move of a Hunter gets Saved.
        private (Vector2Int, Vector2Int, Soldier) _lastMove;

        private void Start()
        {
            // Setup Grid
            grid.MakeGrid();
            // Init
            _isVikingsMove = true; // Vikings Go first
            // Initial Setup
            if (GetComponentInChildren<InitialSetup>())
            {
                GetComponentInChildren<InitialSetup>().Setup(this);
            }
        }

        private void CheckForCaptures()
        {
            if (_lastMove.Item3 == null) return;
            // Get All the squares around the last moved piece that are occupied by an enemy which therefore can be captured
            var enemySquares = grid.GetSquaresAround(_lastMove.Item2).Where(square => !square.IsWater() && square.IsOccupied() && _lastMove.Item3.IsViking() != square.GetPiece().IsViking());
            var lastMovedPieceIsViking = _lastMove.Item3.IsViking();

            foreach (var enemySquare in enemySquares)
            {
                if (grid.GetSquaresAround(enemySquare.GetGridPos()).All(square =>
                        square.IsWater() && !square.IsOccupied() ||
                        square.IsWater() && square.IsOccupied() &&
                        lastMovedPieceIsViking == square.GetPiece().IsViking() ||
                        !square.IsWater() && square.IsOccupied() &&
                        lastMovedPieceIsViking == square.GetPiece().IsViking())
                    ||
                    grid.GetGrid(enemySquare.GetGridPos() + Vector2Int.up).GetPiece().IsViking() ==
                    lastMovedPieceIsViking &&
                    grid.GetGrid(enemySquare.GetGridPos() + Vector2Int.down).GetPiece().IsViking() ==
                    lastMovedPieceIsViking
                    ||
                    grid.GetGrid(enemySquare.GetGridPos() + Vector2Int.left).GetPiece().IsViking() ==
                    lastMovedPieceIsViking &&
                    grid.GetGrid(enemySquare.GetGridPos() + Vector2Int.right).GetPiece().IsViking() ==
                    lastMovedPieceIsViking
                   )
                {
                    enemySquare.GetPiece().Capture();
                }
            }
        }

        public bool IsVikingsMove()
        {
            return _isVikingsMove;
        }

        public void SwitchTurn((Vector2Int, Vector2Int, Soldier) lastMove)
        {
            _isVikingsMove = !_isVikingsMove;
            _lastMove = lastMove;
            
            CheckForCaptures();
            
        }
        // Overload for when no Soldier was moved
        public void SwitchTurn()
        {
            _isVikingsMove = !_isVikingsMove;
        }

        public Board GetGrid()
        {
            return grid;
        }

        public (Vector2Int, Vector2Int, Soldier) GetLastMove()
        {
            return _lastMove;
        }
        public void SetTraitor(Traitor traitor)
        {
            _traitor = traitor;
        }
        public void SetAccomplice(Accomplice accomplice)
        {
            _accomplice = accomplice;
        }
        public Traitor GetTraitor()
        {
            return _traitor;
        }
        public Accomplice GetAccomplice()
        {
            return _accomplice;
        }
    }
}
