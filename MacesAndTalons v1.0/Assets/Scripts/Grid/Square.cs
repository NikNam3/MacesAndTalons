using System;
using UnityEngine;

namespace Scenes.Scripts.Grid
{
    public class Square : IEquatable<Square>
    {
        private int _index;
        private bool _isWater;
        private bool _isOccupied;
        private bool _hasMace;

        private Vector3 _worldPos;

        public Square(int index, bool isWater, Vector3 worldPos)
        {
            _index = index;
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
    }
}
