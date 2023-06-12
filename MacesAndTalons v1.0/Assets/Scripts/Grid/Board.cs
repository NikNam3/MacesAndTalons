using Scenes.Scripts.Grid;
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
        
        private Square[] _grid;

        
        public void MakeGrid()
        {
            _grid = new Square[13 * 13];
        
            for (var i = 0; i < _grid.Length; i++)
            {
                var position = transform.position;
                var x = i % 13;
                var y = Mathf.FloorToInt(i / 13f);
                var worldPos = new Vector3(x * squareSize - squareSize*12 / 2 + position.x, position.y, y * squareSize - squareSize*12 / 2 + position.z);
                _grid[i] = new Square(i, !gridMap.GetPixel(x, y).b.Equals(1f), worldPos);
            }
        }

        public Square[] GetGrid()
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
