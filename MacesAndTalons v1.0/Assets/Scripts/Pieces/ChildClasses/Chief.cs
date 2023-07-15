using System.Linq;
using UnityEngine;

namespace Pieces.ChildClasses
{
    public class Chief : Soldier
    {
        protected override bool MoveTo(Vector2Int gridIndex)
        {
            // Get Neighboured Squares
            var squares = Grid.GetNeighbours(GridIndex);
            // Check if gridIndex is in bounds of the Grid
            if (!Grid.IsInBounds(gridIndex)) return false;
            // Check if the target Square is a neighbour
            if (squares.All(square => square.GetGridPos() != gridIndex)) return false;
            // Check if the square is accessible
            var square = Grid.GetGrid(gridIndex);
            if (square.IsOccupied() || square.IsWater() && !square.HasShipType(isViking)) return false;
            // Prepare for next Move
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
