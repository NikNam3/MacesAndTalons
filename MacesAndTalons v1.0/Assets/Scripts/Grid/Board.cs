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
            
            for (var x = 0; x < _grid.Length / 13 - 1; x++)
            {
                for (var y = 0; y < _grid.Length / - 1; x++)
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
