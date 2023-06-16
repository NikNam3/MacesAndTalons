using System;
using Grid;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace Pieces.ChildClasses
{
    public class Hunter : Soldier
    {
        public override bool MoveTo(Vector2Int gridIndex)
        {
            // Pre-checks: 
            // Test if on the same row our column
            if (!(GridIndex.x == gridIndex.x || GridIndex.y == gridIndex.y)) return false;
            // Get Squares between
            throw new NotImplementedException("Hunter Not Finished");
        }
    }
}
