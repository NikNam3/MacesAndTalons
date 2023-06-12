using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Pieces;
using Pieces.ChildClasses;
using Scenes.Scripts.Grid;
using Unity.VisualScripting;
using UnityEngine;

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
        Spawn(marauderChief, 0, new Vector3(0f, 0f, 0f));
    }

    // Update is called once per frame
    private void Spawn(GameObject obj, int gridIndex, Vector3 rotation)
    {
        // Instantiate Object
        var script = Instantiate(obj, grid.GetGrid()[gridIndex].GetWorldPos(), Quaternion.Euler(rotation))
            .GetComponent<Piece>();
        // Initialise Object
        script.Init(grid, gridIndex);
    }
}
