using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Tile : BaselineManager
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public bool isTemporary = false;
    public List<Tile> surroundingHouses = new();
    public int level;
    public TileType tileType;
    public int output;
    public WorkRequirement workRequirement;

    public void SetRaycast()
    {
        image.raycastTarget = !image.raycastTarget;
    }

    public void CheckSurrounding()
    {
        // Define offsets for all 8 directions
        Vector2Int[] directions = new Vector2Int[]
        {
            new(-1, -1), // bottom left
            new(0, -1),  // bottom
            new(1, -1),  // bottom right
            new(-1, 0),  // left
            new(1, 0),   // right
            new(-1, 1),  // top left
            new(0, 1),   // top
            new(1, 1)    // top right
        };

        // Get the position of this tile in the grid
        Vector2Int thisPosGrid = gameManager.GetPosition(this, false);
        Vector2Int thisPos = gameManager.GetPosition(this, true);

        // Clear the surroundingHouses list
        surroundingHouses.Clear();

        // For each direction, get the neighboring tile
        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = thisPos + dir;

            // Check if the neighboring position is within the grid and both coordinates are non-negative
            if (CheckBoundary(neighborPos.x , neighborPos.y) && gameManager.tileDictionary.ContainsKey(neighborPos))
            {
                // Get the neighboring tile
                Tile neighborTile = gameManager.tileDictionary[neighborPos];

                // Add the neighboring tile to the surroundingHouses list
                surroundingHouses.Add(neighborTile);
            }
        }
    }

    private bool CheckBoundary(int x, int y) 
    {
        return x >= 0 && y >= 0 && x < 4 && y < 4;
    }

    public void Upgrade()
    {
        // Check if the tile can be upgraded
        if (level < gameManager.tileConfig.tileData[tileType].maxLevel)
        {
            // Increase the level of the tile
            level++;

            // Update the tile's image
            image.sprite = gameManager.tileConfig.tileData[tileType].sprites[level];

            // Update the tile's output
            output = gameManager.tileConfig.tileData[tileType].output[level];
        }
    }
}

[Serializable]
public enum TileType {
    Lumberjack,
    SheepFarm,
    Forger,
    Smelter,
    House,
    Barrack,
    WeatherMachine
}
