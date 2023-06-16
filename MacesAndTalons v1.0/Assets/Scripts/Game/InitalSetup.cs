using Grid;
using Pieces;
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
            Spawn(marauderChief, new Vector2Int(7, 7), new Vector3(0f, 0f, 0f));
        }

        // Update is called once per frame
        private void Spawn(GameObject obj, Vector2Int gridIndex, Vector3 rotation)
        {
            // Instantiate Object
            var script = Instantiate(obj, grid.GetGrid()[gridIndex.x, gridIndex.y].GetWorldPos(), Quaternion.Euler(rotation)).GetComponent<Piece>();
            // Initialise Object
            script.Init(grid, gridIndex);
        }
    }
}
