using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Pieces.ChildClasses
{
    public class Hunter : Piece
    {
        public override bool MoveTo(Vector2Int gridIndex)
        {
            // Check if on the same row or column (Can only move on a straight line)
            if (GridIndex.x != gridIndex.x && GridIndex.y != gridIndex.y) return false;
            // Check if gridIndex is in bounds of the Grid
            if (!Grid.IsInBounds(gridIndex)) return false;
            // Check if all squares between are accessible
            var squares = Grid.GetSquaresBetween(GridIndex, gridIndex);
            foreach (var square in squares)
            {
                if (square.IsOccupied()) return false;
                if (square.IsWater() && !square.IsShip(isViking)) return false;
            }
            return true;

        }
    }
}
