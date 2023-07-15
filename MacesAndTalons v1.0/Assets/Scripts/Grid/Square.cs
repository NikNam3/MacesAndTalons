using System;
using Pieces;
using UnityEngine;

namespace Grid
{
    public class Square : IEquatable<Vector2Int>
    {
        
        private bool _isOccupied; // Only True if A Soldier is on The field (For water a Soldier must be on a ship)
        private Piece _piece;
        
        private bool _hasMace;
        private bool _hasVikingShip;
        private bool _hasMarauderShip;
        
        

        private readonly Vector3 _worldPos;
        private readonly Vector2Int _gridPos;
        private readonly bool _isWater;

        public Square(Vector2Int gridPos, bool isWater, Vector3 worldPos)
        {
            _gridPos = gridPos;
            _isWater = isWater;
            _isOccupied = false;
            _hasMace = false;
            _piece = null;
            _worldPos = worldPos;

        }

        public bool Equals(Vector2Int other)
        {
            return _gridPos == other;
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
        public void Occupy(Piece piece)
        {
            _piece = piece;
            _isOccupied = true;
        }
        public void UnOccupy()
        {
            _piece = null;
            _isOccupied = false;
        }

        public void SetShipType(bool isViking)
        {
            if (isViking)
            {
                _hasVikingShip = true;
            }
            else
            {
                _hasMarauderShip = true;
            }
        }
        public void ResetShipType()
        {
            _hasVikingShip = false;
            _hasMarauderShip = false;
        }

        public bool HasShipType(bool isViking)
        {
            return _hasVikingShip && isViking || _hasMarauderShip && !isViking;
        }
        public bool HasMace()
        {
            return _hasMace;
        }

        public Vector2Int GetGridPos()
        {
            return _gridPos;
        }
        public Piece GetPiece()
        {
            return _piece;
        }
    }
}
