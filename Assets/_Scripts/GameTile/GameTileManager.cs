using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

public class GameTileManager : BaselineManager
{
    public GameObject[,] grid = new GameObject[5, 5];
    public SerializedDictionary<Vector2Int,GameObject> tileDictionary; 
    public Transform tileParent;

    public GameObject currentHoldingTile;
    public void Start(){
        gameManager = this;

        tileDictionary = new SerializedDictionary<Vector2Int, GameObject>();
        int gridWidth = grid.GetLength(0); // Assuming the grid is a square
        for (int i = 0; i < tileParent.childCount; i++)
        {
            GameObject tile = tileParent.GetChild(i).gameObject;
            int x = i % gridWidth;
            int y = i / gridWidth;
            Vector2Int tileLocation = new(x, y);
            grid[x, y] = tile;
            tileDictionary.Add(tileLocation, tile);
        }
    }

    [Button]
    public void GetRandomTile() {
        int x = UnityEngine.Random.Range(1, 5);
        int y = UnityEngine.Random.Range(1, 5);
        Vector2Int tileLocation = new(x, y);

        if (tileDictionary.ContainsKey(tileLocation)) {
            GameObject tile = tileDictionary[tileLocation];
            Debug.Log("Tile at " + x + ", " + y + " is " + tile.name);
            GameObject randomTile = grid[x, y];
            Debug.Log("Tile at " + x + ", " + y + " Array is " + randomTile.name);
        } else {
            Debug.Log("No tile found at " + x + ", " + y);
        }
    }
}
