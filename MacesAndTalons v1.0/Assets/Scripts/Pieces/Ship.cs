using Grid;
using UnityEngine;

namespace Pieces
{
    internal abstract class Ship : Piece
    {
        private char _orientation; // {N, E, S, W} North East South West
        private Transform[] _slots;
        
        public void Init(Board grid, int gridIndex, char orientation)
        {
            Grid = grid;
            GridIndex = gridIndex;
            _orientation = orientation;
        }
        
        public abstract override bool MoveTo(int gridIndex);
    }
}
