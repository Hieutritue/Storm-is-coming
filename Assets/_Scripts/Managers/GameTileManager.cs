using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class GameTileManager : MonoBehaviour
{
    public List<Slot> Slots;
    public List<Tile> Tiles;

    public GameObject[,] grid = new GameObject[4, 4];
    public SerializedDictionary<Vector2Int, GameObject> gridDictionary;

    public Tile[,] gridTile = new Tile[4, 4];
    public SerializedDictionary<Vector2Int, Tile> tileDictionary;

    public Transform tileParent;

    public GameObject currentHoldingTile;

    public TileConfig tileConfig;

    public WindDirection windDirection;

    #region ldminh

    public void Start()
    {
        gridDictionary = new SerializedDictionary<Vector2Int, GameObject>();
        Slots = new List<Slot>();
        Tiles = new List<Tile>();

        int gridWidth = grid.GetLength(0); // Assuming the grid is a square
        for (int i = 0; i < tileParent.childCount; i++)
        {
            GameObject tile = tileParent.GetChild(i).gameObject;
            int x = i % gridWidth;
            int y = i / gridWidth;

            Vector2Int tileLocation = new(x, y);
            grid[x, y] = tile;
            gridDictionary.Add(tileLocation, tile);

            int hieuX = 3 - i % gridWidth;
            int hieuY = i / gridWidth;

            var slot = tile.GetComponent<Slot>();
            slot.Pos = new Pos(hieuX, hieuY);
            Slots.Add(slot);
            
            Debug.Log($"{slot.gameObject}: {hieuX}, {hieuY}");

            if (tile.transform.childCount > 0)
            {
                Tile tileComponent = tile.transform.GetChild(0).GetComponent<Tile>();
                gridTile[x, y] = tileComponent;
                tileDictionary.Add(tileLocation, tileComponent);
            }
        }

        CheckForTile();
    }

    public void CheckForTile()
    {
        foreach (var grid in gridDictionary)
        {
            if (grid.Value.transform.childCount > 0)
            {
                Tile tile = grid.Value.transform.GetChild(0).GetComponent<Tile>();
                Vector2Int tileLocation = grid.Key;
                if (gridTile[tileLocation.x, tileLocation.y] == null) // Check if the tile already exists
                {
                    gridTile[tileLocation.x, tileLocation.y] = tile;
                }

                if (!tileDictionary.ContainsKey(tileLocation)) // Check if the tile already exists
                {
                    tileDictionary.Add(tileLocation, tile);
                }
            }
        }
    }
    //
    // public void CheckAllTile()
    // {
    //     foreach (var g in gridDictionary.Values)
    //     {
    //         if (g.transform.childCount > 0)
    //         {
    //             Tile tile = g.transform.GetChild(0).GetComponent<Tile>();
    //             tile.CheckSurrounding();
    //         }
    //     }
    // }

    public Vector2Int GetPosition(Tile tile, bool useDict)
    {
        switch (useDict)
        {
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
    public void GetRandomTile()
    {
        int x = UnityEngine.Random.Range(1, 5);
        int y = UnityEngine.Random.Range(1, 5);
        Vector2Int tileLocation = new(x, y);

        if (gridDictionary.ContainsKey(tileLocation))
        {
            GameObject tile = gridDictionary[tileLocation];
            Debug.Log("Tile at " + x + ", " + y + " is " + tile.name);
            GameObject randomTile = grid[x, y];
            Debug.Log("Tile at " + x + ", " + y + " Array is " + randomTile.name);
        }
        else
        {
            Debug.Log("No tile found at " + x + ", " + y);
        }
    }

    public int GetTileDistance()
    {
        return -1;
    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) ShiftByWind(new Vector2Int(-1, 0));
        if (Input.GetKeyDown(KeyCode.RightArrow)) ShiftByWind(new Vector2Int(1, 0));
        if (Input.GetKeyDown(KeyCode.UpArrow)) ShiftByWind(new Vector2Int(0, -1));
        if (Input.GetKeyDown(KeyCode.DownArrow)) ShiftByWind(new Vector2Int(0, 1));
    }

    public void ShiftByWind(Vector2Int dir)
    {
        var orderedTiles = Tiles.OrderBy(s => s.Pos.X).ThenBy(s => s.Pos.Y).ToList();
        if (dir == Vector2Int.up || dir == Vector2Int.right)
            orderedTiles.Reverse();
        foreach (var tile in orderedTiles)
        {
            Slot possibleSlot;
            var slotToShiftTo = tile.OccupiedSlot;
            do
            {
                tile.SetSlot(slotToShiftTo);
                possibleSlot = GetSlotByPos(slotToShiftTo.Pos.X + dir.x, slotToShiftTo.Pos.Y + dir.y);
                if (possibleSlot) //not out of bound
                {
                    if (!possibleSlot.CurrentTile) //slot empty
                        slotToShiftTo = possibleSlot;
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            } while (true);

            possibleSlot = GetSlotByPos(slotToShiftTo.Pos.X + dir.x, slotToShiftTo.Pos.Y + dir.y);
            if (possibleSlot && tile.CanMergeWith(possibleSlot.CurrentTile))
            {
                slotToShiftTo = possibleSlot;
                tile.SetSlot(slotToShiftTo);
            }
            
            tile.transform.SetParent(slotToShiftTo.transform);
            tile.transform.localPosition = Vector3.zero;

            if (slotToShiftTo.transform.childCount == 2)
            {
                var oldTile = slotToShiftTo.transform.GetChild(0).gameObject;
                GameManager.Instance.GameTileManager.Tiles.Remove(oldTile.GetComponent<Tile>());
                Destroy(oldTile);
                
                tile.Upgrade();
            }
        }
    }

    public Tile GetTileByPos(int x, int y)
        => GetSlotByPos(x, y).CurrentTile;

    public Slot GetSlotByPos(int x, int y)
        => Slots.FirstOrDefault(s => s.Pos.X == x && s.Pos.Y == y);
}

[Serializable]
public enum WindDirection
{
    Up,
    Down,
    Left,
    Right
}