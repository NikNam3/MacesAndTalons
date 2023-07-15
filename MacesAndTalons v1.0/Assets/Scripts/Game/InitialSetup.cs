using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Pieces;
using Pieces.ChildClasses;
using UnityEngine;

namespace Game
{
    public class InitialSetup : MonoBehaviour
    {
        [Header("Pieces")]
        [SerializeField] private GameObject vikingHunter;
        [SerializeField] private GameObject vikingChief;
        [SerializeField] private GameObject vikingLongShip;
        [SerializeField] private GameObject vikingKingShip;
        [SerializeField] private GameObject marauderHunter;
        [SerializeField] private GameObject marauderChief;
        [SerializeField] private GameObject marauderLongShip;
        [SerializeField] private GameObject marauderKingShip;
        [SerializeField] private GameObject traitor;
        [SerializeField] private GameObject accomplice;
        [SerializeField] private GameObject dragon;
        [SerializeField] private GameObject mace;
        [Header("System")]
        [SerializeField] private Board grid;
        private Controller _controller;
        
        public void Setup(Controller controller)
        {
            _controller = controller;
            // Generate Grid (Map)
            grid.MakeGrid();
            
            
            
            // Spawn Pieces
            Spawn(vikingHunter, new Vector2Int(6, 0), new Vector3(0f, 0f, 0f));
            
            // Spawn Special Pieces
            
            
            // Spawn Ships
            foreach (var pos in Init.Positions)
            {
                var ship = pos.Item3 switch
                {
                    true when pos.Item4 => vikingLongShip,
                    false when pos.Item4 => vikingKingShip,
                    true => marauderLongShip,
                    _ => marauderKingShip
                };
                SpawnShip(ship, pos.Item1, pos.Item2, pos.Item4);
            }
        }
        
        private GameObject Spawn(GameObject obj, Vector2Int gridIndex, Vector3 rotation)
        {
            // Instantiate Object
            var piece = Instantiate(obj, grid.GetGrid(gridIndex).GetWorldPos(), Quaternion.Euler(rotation));
            
            // Get Init Script
            var script = piece.GetComponent<Piece>();
            
            grid.GetGrid(gridIndex).Occupy(script);
            // Initialise Object
            script.Init(grid, gridIndex, _controller);
            return piece;
        }

        private GameObject SpawnShip(GameObject obj, Vector2Int pos1, Vector2Int pos2, bool isViking)
        {
            // Instantiate obj in the middle of pos1 and pos2 rotated so its facing pos2
            var rotation = (pos1.x == pos2.x) switch
            {
                true => new Vector3(0, 90, 0),
                _ => new Vector3(0, 0, 0)
            };
            var pos = (grid.GetGrid(pos1).GetWorldPos() + grid.GetGrid(pos2).GetWorldPos()) / 2;
            
            var piece = Instantiate(obj, pos, Quaternion.Euler(rotation));
            // Get Init Script
            var script = piece.GetComponent<Ship>();
            // Initialise Object
            script.Init(grid, pos1, pos2, _controller);
            // Set Occupied Squares
            grid.GetSquaresBetween(pos1, pos2).ForEach(x => x.Occupy(script));
            grid.GetSquaresBetween(pos1, pos2).ForEach(x => x.SetShipType(isViking));
            return piece;
        }
    }
}
