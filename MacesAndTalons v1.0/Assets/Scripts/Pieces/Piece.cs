using System;
using Game;
using Grid;
using UnityEngine;

namespace Pieces
{
   public abstract class Piece : MonoBehaviour
   {
      [SerializeField] 
      protected internal bool isViking;

      protected bool Selected;
      protected Board Grid;
      protected Vector2Int GridIndex;
      protected bool IsMoving;
      protected Controller GameController;
      protected Camera Camera;


      private void Start()
      {
         Camera = Camera.main;
      }

      internal void Select()
      {
         Selected = true;
      }

      protected void Unselect()
      {
         Selected = false;
      }

      public abstract void Capture();

      public bool IsViking()
      {
         return isViking;
      }

      protected void SetIsMoving(bool isMoving)
      {
         IsMoving = isMoving;
      }

      public void Init(Board grid, Vector2Int gridIndex, Controller gameController)
      {
         Grid = grid;
         GridIndex = gridIndex;
         GameController = gameController;
      }
      public Vector2Int GetGridIndex()
      {
         return GridIndex;
      }
   }
}
