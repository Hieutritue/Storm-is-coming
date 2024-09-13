using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class GameTileManager : BaselineManager
{
    public GameObject[,] grid = new GameObject[5, 5];
    public SerializedDictionary<Vector2Int,GameObject> gridDictionary; 
    
    public Tile[,] gridTile = new Tile[5, 5];
    public SerializedDictionary<Vector2Int,Tile> tileDictionary;
    
    public Transform tileParent;

    public GameObject currentHoldingTile;

    public TileConfig tileConfig;
    public void Start(){
        gameManager = this;

        gridDictionary = new SerializedDictionary<Vector2Int, GameObject>();
        int gridWidth = grid.GetLength(0); // Assuming the grid is a square
        for (int i = 0; i < tileParent.childCount; i++)
        {
            GameObject tile = tileParent.GetChild(i).gameObject;
            int x = i % gridWidth;
            int y = i / gridWidth;
            Vector2Int tileLocation = new(x, y);
            grid[x, y] = tile;
            gridDictionary.Add(tileLocation, tile);
            
            if (tile.transform.childCount > 0)
            {
                Tile tileComponent = tile.transform.GetChild(0).GetComponent<Tile>();
                gridTile[x, y] = tileComponent;
                tileDictionary.Add(tileLocation, tileComponent);
            }
        }
    }

    public void CheckAllTile()
    {
        foreach (var g in gridDictionary.Values)
        {
            if (g.transform.childCount > 0)
            {
                Tile tile = g.transform.GetChild(0).GetComponent<Tile>();
                tile.CheckSurrounding();
            }
        }
    }

    public Vector2Int GetPosition(Tile tile, bool useDict){
        switch (useDict) {
            case false:
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        if (grid[x, y] == tile.gameObject)
                        {
                            return new Vector2Int(x, y);
                        }
                    }
                }
                break;
            case true:
                foreach (var kvp in tileDictionary)
                {
                    if (kvp.Value == tile)
                    {
                        return kvp.Key;
                    }
                }
                break;
        }

        return new Vector2Int(-1, -1);
    }

    [Button]
    public void GetRandomTile() {
        int x = UnityEngine.Random.Range(1, 5);
        int y = UnityEngine.Random.Range(1, 5);
        Vector2Int tileLocation = new(x, y);

        if (gridDictionary.ContainsKey(tileLocation)) {
            GameObject tile = gridDictionary[tileLocation];
            Debug.Log("Tile at " + x + ", " + y + " is " + tile.name);
            GameObject randomTile = grid[x, y];
            Debug.Log("Tile at " + x + ", " + y + " Array is " + randomTile.name);
        } else {
            Debug.Log("No tile found at " + x + ", " + y);
        }
    }
}
