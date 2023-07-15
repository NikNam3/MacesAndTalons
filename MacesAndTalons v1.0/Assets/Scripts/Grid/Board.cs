using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Grid
{
    public class Board : MonoBehaviour
    {
        [SerializeField]
        private Texture2D gridMap;
        [SerializeField]
        private float squareSize;
        
        private Square[,] _grid;

        
        public void MakeGrid()
        {
            _grid = new Square[13,13];
            for (var x = 0; x < _grid.Length / 13; x++) {
                for (var y = 0; y < _grid.Length / 13; y++)
                {
                    var position = transform.position;
                    var worldPos = new Vector3(x * squareSize - squareSize*12 / 2 + position.x, position.y, y * squareSize - squareSize*12 / 2 + position.z);
                    _grid[x, y] = new Square(
                        new Vector2Int(x, y), 
                        gridMap.GetPixel(x, y).b.Equals(1f),
                        worldPos);
                }
            }
        }

        public Square[,] GetGrid()
        {
            return _grid;
        }

        public Square GetGrid(Vector2Int gridIndex)
        {
            return !IsInBounds(gridIndex) ? null : _grid[gridIndex.x, gridIndex.y];
        }

        public Vector2Int GetGridIndexFromWorldPos(Vector3 worldPosition)
        {
            var gridWorldSize = new Vector2(13 * squareSize, 13 * squareSize);
            var pos = transform.position;
            var percentX = (worldPosition.x-pos.x + gridWorldSize.x/2) / gridWorldSize.x;
            var percentY = (worldPosition.z-pos.z + gridWorldSize.y/2) / gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            
            var x = Mathf.RoundToInt((12)*percentX);
            var y = Mathf.RoundToInt((12)*percentY);
            return new Vector2Int(x, y);
        }
        public Vector2Int GetGridIndexFromMousePos()
        {
            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition); // There Must always be a Main Camera therefore it cant be null
            return Physics.Raycast(ray, out var hit) ? GetGridIndexFromWorldPos(hit.point) : Vector2Int.zero;
        }
        

        public List<Square> GetSquaresBetween(Vector2Int point1, Vector2Int point2)
        {
            var squares = new List<Square>();
            var x0 = point1.x;
            var y0 = point1.y;
            var x1 = point2.x;
            var y1 = point2.y;
            var steep = Mathf.Abs(y1 - y0) > Mathf.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }

            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            var dx = x1 - x0;
            var dy = Mathf.Abs(y1 - y0);
            var error = dx / 2;
            var yStep = y0 < y1 ? 1 : -1;
            var y = y0;
            for (var x = x0; x <= x1; x++)
            {
                if (steep)
                {
                    if (IsInBounds(y, x)) squares.Add(_grid[y, x]);
                }
                else
                {
                    if (IsInBounds(x, y)) squares.Add(_grid[x, y]);
                }

                error -= dy;
                if (error >= 0) continue;
                y += yStep;
                error += dx;
            }
            //squares.RemoveAt(0); // Remove the Home square as it does not have to be traversed again
            return squares;
        }

        public List<Square> GetNeighbours(Vector2Int gridIndex)
        {
            var squares = new List<Square>();

            for (var dx = -1; dx <= 1; dx++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    if (dy == 0 && dx == 0) continue;
                    if (!IsInBounds(gridIndex.x+dx, gridIndex.y+dy)) continue;
                    squares.Add(_grid[gridIndex.x+dx, gridIndex.y+dy]);
                }
            }

            return squares;
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            (a, b) = (b, a);
        }

        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < _grid.GetLength(0) && y >= 0 && y < _grid.GetLength(1);
        }
        public bool IsInBounds(Vector2Int gridIndex)
        {
            var x = gridIndex.x;
            var y = gridIndex.y;
            return x >= 0 && x < _grid.GetLength(0) && y >= 0 && y < _grid.GetLength(1);
        }

        public List<Square> GetSquaresAround(Vector2Int pos)
        {
            var squares = new List<Square>();
            for (var dx = -1; dx <= 1; dx++)
            {
                if (!IsInBounds(pos.x +dx, pos.y)) continue;
                squares.Add(_grid[pos.x+dx, pos.y]);
            }
            for (var dy = -1; dy <= 1; dy++)
            {
                if (!IsInBounds(pos.x, pos.y+dy)) continue;
                squares.Add(_grid[pos.x, pos.y+dy]);
            }
            return squares;
        }
        
        private void OnDrawGizmos()
        {
            if (_grid.IsUnityNull()) return;
            foreach (var square in _grid)
            {
                Gizmos.color = Color.white;
                Gizmos.color = square.IsWater() ? Color.blue : Gizmos.color;
                Gizmos.color = square.IsOccupied() ? Color.red : Gizmos.color;
                Gizmos.color = square.HasShipType(true) ? Color.green : Gizmos.color;
                Gizmos.color = square.HasShipType(false) ? Color.yellow : Gizmos.color;
                Gizmos.color = square.HasMace() ? Color.magenta : Gizmos.color;
                Gizmos.color = GetGridIndexFromMousePos() == square.GetGridPos() ? Color.cyan : Gizmos.color;
                Gizmos.DrawCube(square.GetWorldPos(), new Vector3(squareSize, 1f, squareSize));
            }
        
        }
        
    }
}
