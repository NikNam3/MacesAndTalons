using Grid;
using UnityEngine;

namespace Pieces
{
   public abstract class Piece : MonoBehaviour
   {
      [SerializeField] 
      protected bool isViking;
      protected Board Grid;
      protected int GridIndex;

      public void Init(Board grid, int gridIndex)
      {
         Grid = grid;
         GridIndex = gridIndex;
      }
      public abstract bool MoveTo(int gridIndex);

   }
}
