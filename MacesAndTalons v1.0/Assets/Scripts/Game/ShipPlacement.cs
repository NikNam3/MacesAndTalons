using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using Pieces;
using Pieces.ChildClasses;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Game
{
    public class ShipPlacement : MonoBehaviour
    {
        [SerializeField] private GameObject longShipDummy;
        [SerializeField] private GameObject kingShipDummy;
        
        private Board _grid;
        private Controller _controller;
        // Pos1, Pos2, IsLongShip, IsViking
        private List<(Vector2Int, Vector2Int, bool, bool)> _positions = new();

        private Vector2Int _pos1 = new(-1, -1);
        private Vector2Int _pos2 = new(-1, -1);

        private bool _longShipPlaced;
        private bool _shortShipPlaced;

        private void Start()
        {
            _controller = GetComponentInParent<Controller>();
            _grid = _controller.GetGrid();
        }

        // Update is called once per frame
        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            var gridPos = _grid.GetGridIndexFromMousePos();
            
            // Check if Position is valid
            if (!_grid.GetGrid(gridPos).IsWater() || 
                _grid.GetGrid(gridPos).IsOccupied() ||
                _controller.IsVikingsMove() && gridPos.y >= 6 ||
                !_controller.IsVikingsMove() && gridPos.y <= 6
               ) return;

            // Try to place ships
            if (!SetupPositions(gridPos)) return;
            TryToPlaceShip(ref _longShipPlaced, 2);
            TryToPlaceShip(ref _shortShipPlaced, 1);
            
            // Reset Positions
            _pos1 = new Vector2Int(-1, -1);
            _pos2 = new Vector2Int(-1, -1);
            
            // If Long and Short ship are placed => Switch Turn
            if (!_longShipPlaced || !_shortShipPlaced) return;
            
            _longShipPlaced = false;
            _shortShipPlaced = false;
            _controller.SwitchTurn();
            
            // If its the Vikings turn => All Ships are placed
            if (!_controller.IsVikingsMove()) return;
            
            Init.Positions = _positions;
            SceneManager.LoadScene("Test");


        }

        private bool SetupPositions(Vector2Int gridPos)
        {
            if (!_grid.IsInBounds(_pos1))
            {
                _pos1 = gridPos;
                return false;
            }
            if (!_grid.IsInBounds(_pos2) && gridPos != _pos1)
            {
                _pos2 = gridPos;
                return true;
            }

            return false;
        }

        private void TryToPlaceShip(ref bool shipPlaced, int length)
        {
            // If: on the same row or column AND distance = length AND all squares are not Occupied AND this ship has not been placed => continue And Ship is on the right side
            if ((_pos1.x != _pos2.x && _pos1.y != _pos2.y) ||
                (Mathf.Abs(_pos1.x - _pos2.x) != length && Mathf.Abs(_pos1.y - _pos2.y) != length) || 
                _grid.GetSquaresBetween(_pos1, _pos2).Any(x => x.IsOccupied()) ||
                shipPlaced) return;
            
            _positions.Add((_pos1, _pos2, length == 2, _controller.IsVikingsMove()));
            _grid.GetSquaresBetween(_pos1, _pos2).ForEach(x => x.Occupy(null));
            
            var ship = (length == 2) switch
            {
                true => longShipDummy,
                _ => kingShipDummy
            };

            SpawnShip(ship, _pos1, _pos2);
            shipPlaced = true;
        }
        private void SpawnShip(GameObject obj, Vector2Int pos1, Vector2Int pos2)
        {
            // Instantiate obj in the middle of pos1 and pos2 rotated so its facing pos2
            var rotation = (pos1.x == pos2.x) switch
            {
                true => new Vector3(0, 90, 0),
                _ => new Vector3(0, 0, 0)
            };
            var pos = (_grid.GetGrid(pos1).GetWorldPos() + _grid.GetGrid(pos2).GetWorldPos()) / 2;
            
            Instantiate(obj, pos, Quaternion.Euler(rotation));

        }
    }
}
