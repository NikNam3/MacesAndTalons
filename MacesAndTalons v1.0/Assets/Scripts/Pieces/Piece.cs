using Grid;
using UnityEngine;

namespace Pieces
{
   public abstract class Piece : MonoBehaviour
   {
      [SerializeField] 
      protected bool isViking;
      protected Board Grid;
      protected Vector2Int GridIndex;

      public void Init(Board grid, Vector2Int gridIndex)
      {
         Grid = grid;
         GridIndex = gridIndex;
      }
      public abstract bool MoveTo(Vector2Int gridIndex);

   }
}
