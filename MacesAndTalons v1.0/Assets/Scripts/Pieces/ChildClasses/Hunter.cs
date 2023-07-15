using System.Linq;
using UnityEngine;

namespace Pieces.ChildClasses
{
    public class Hunter : Soldier
    {
        protected override bool MoveTo(Vector2Int gridIndex)
        {
            // Check if on the same row or column (Can only move on a straight line)
            if (GridIndex.x != gridIndex.x && GridIndex.y != gridIndex.y) return false;
            // Check if gridIndex is in bounds of the Grid
            if (!Grid.IsInBounds(gridIndex)) return false;
            // Check if all squares between are accessible
            var squares = Grid.GetSquaresBetween(GridIndex, gridIndex);
            foreach (var square in squares.Where(square => square.GetGridPos() != GridIndex))
            {
                if (square.IsOccupied() && !square.IsWater()) return false;
                if (square.IsWater() && !square.HasShipType(isViking)) return false;
            }

            if (Grid.GetGrid(gridIndex).IsWater()) return false;
            Unselect();
            GameController.SwitchTurn((GridIndex, gridIndex, this));
            // Update Position
            Grid.GetGrid(GridIndex).UnOccupy();
            Grid.GetGrid(gridIndex).Occupy(this);
            // Movement Anim
            StartCoroutine(MovementAnim(Grid.GetGrid(GridIndex).GetWorldPos(), Grid.GetGrid(gridIndex).GetWorldPos()));
            GridIndex = gridIndex;
            
            
            return true;
            
        }
    }
}
