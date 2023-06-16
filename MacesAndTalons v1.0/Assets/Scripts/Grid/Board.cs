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
            return _grid[gridIndex.x, gridIndex.y];
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
                Swap(ref x0,
                    ref y0);
                Swap(ref x1,
                    ref y1);
            }

            if (x0 > x1)
            {
                Swap(ref x0,
                    ref x1);
                Swap(ref y0,
                    ref y1);
            }

            var dx = x1 - x0;
            var dy = Mathf.Abs(y1 - y0);
            var error = dx / 2;
            var yStep = y0 < y1
                ? 1
                : -1;
            var y = y0;

            for (var x = x0;
                 x <= x1;
                 x++)
            {
                if (steep)
                {
                    if (IsInBounds(y,
                            x))
                        squares.Add(_grid[y,
                            x]);
                }
                else
                {
                    if (IsInBounds(x,
                            y))
                        squares.Add(_grid[x,
                            y]);
                }

                error -= dy;
                if (error >= 0) continue;
                y += yStep;
                error += dx;
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
        private void OnDrawGizmos()
        {
            if (_grid.IsUnityNull()) return;
            foreach (var square in _grid)
            {
                Gizmos.color = square.IsWater() ? Color.blue : Color.black;
                Gizmos.DrawCube(square.GetWorldPos(), new Vector3(squareSize, 1f, squareSize));
            }
        
        }
    }
}
