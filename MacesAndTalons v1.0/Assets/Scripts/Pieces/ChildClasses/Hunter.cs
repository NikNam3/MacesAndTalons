using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;

namespace Pieces.ChildClasses
{
    public class Hunter : Soldier
    {
        private Vector3 _lerpedValue;
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
            StartCoroutine(Lerp(Grid.GetGrid(GridIndex).GetWorldPos(), Grid.GetGrid(gridIndex).GetWorldPos()));
            return true;
            
        }

        private IEnumerator Lerp(Vector3 start, Vector3 end)
        {
            float timeElapsed = 0;
            
            while (timeElapsed < movementDuration)
            {
                var t = timeElapsed / movementDuration;

                t = animCurve.Evaluate(t);
                _lerpedValue = Vector3.Lerp(start, end, t);
                transform.position = _lerpedValue;
                timeElapsed += Time.deltaTime;
            
                yield return null;
            }

            _lerpedValue = end;

        }
    }
}
