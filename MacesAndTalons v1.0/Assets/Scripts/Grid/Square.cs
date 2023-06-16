using System;
using UnityEngine;

namespace Grid
{
    public class Square : IEquatable<Square>
    {
        private Vector2Int _gridPos;
        private bool _isWater;
        private bool _isOccupied; // Only True if A Soldier is on The field (For water a Soldier must be on a ship)
        private bool _hasMace;
        private bool _hasVikingShip;
        private bool _hasMarauderShip;

        private Vector3 _worldPos;

        public Square(Vector2Int gridPos, bool isWater, Vector3 worldPos)
        {
            _gridPos = gridPos;
            _isWater = isWater;
            _isOccupied = false;
            _hasMace = false;
            _worldPos = worldPos;

        }

        public bool Equals(Square other)
        {
            return false;
        }

        public Vector3 GetWorldPos()
        {
            return _worldPos;
        }

        public bool IsWater()
        {
            return _isWater;
        }

        public bool IsOccupied()
        {
            return _isOccupied;
        }

        public bool IsShip(bool isViking)
        {
            return _hasVikingShip && isViking || _hasMarauderShip && !isViking;
        }
    }
}
