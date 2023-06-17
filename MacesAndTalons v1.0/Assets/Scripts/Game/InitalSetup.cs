using Grid;
using Pieces;
using Pieces.ChildClasses;
using UnityEngine;

namespace Game
{
    public class InitalSetup : MonoBehaviour
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

        private void Start()
        {
            // Generate Grid (Map)
            grid.MakeGrid();
        
            // Spawn Pieces
            Spawn(vikingHunter, new Vector2Int(12, 12), new Vector3(0f, 0f, 0f));
        }

        // Update is called once per frame
        private GameObject Spawn(GameObject obj, Vector2Int gridIndex, Vector3 rotation)
        {
            // Instantiate Object
            var piece = Instantiate(obj, grid.GetGrid(gridIndex).GetWorldPos(), Quaternion.Euler(rotation));
            // Get Init Script
            var script = piece.GetComponent<Piece>();
            // Initialise Object
            script.Init(grid, gridIndex);
            return piece;
        }
    }
}
