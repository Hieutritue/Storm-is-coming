using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class GameTileManager : MonoBehaviour
{
    public List<Slot> Slots;
    public List<Tile> Tiles;

    public GameObject[,] grid = new GameObject[4, 4];
    public SerializedDictionary<Vector2Int, GameObject> gridDictionary;

    public Tile[,] gridTile = new Tile[4, 4];
    public SerializedDictionary<Vector2Int, Tile> tileDictionary;

    public Transform tileParent;

    public Tile currentHoldingTile;

    public TileConfig tileConfig;

    public WindDirection windDirection;

    private float _travelTime = .2f;
    private bool _moving;

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

    }


    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) ShiftByWind(new Vector2Int(-1, 0));
        if (Input.GetKeyDown(KeyCode.RightArrow)) ShiftByWind(new Vector2Int(1, 0));
        if (Input.GetKeyDown(KeyCode.UpArrow)) ShiftByWind(new Vector2Int(0, -1));
        if (Input.GetKeyDown(KeyCode.DownArrow)) ShiftByWind(new Vector2Int(0, 1));
    }

    public void CallThunder()
    {
        var slotToZap = Slots[new Random().Next(0, Slots.Count - 1)];
        
        //TODO: visual
        
        Destroy(slotToZap.CurrentTile.gameObject);
    }

    public void ShiftByWind(Vector2Int dir)
    {
        if(_moving) return;
         
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
            
            if (slotToShiftTo.transform.childCount == 2)
            {
                var oldTile = slotToShiftTo.transform.GetChild(0).gameObject;
                GameManager.Instance.GameTileManager.Tiles.Remove(oldTile.GetComponent<Tile>());
                Destroy(oldTile);
                
                tile.Upgrade();
            }

            _moving = true;
            tile.transform.DOMove(slotToShiftTo.transform.position, _travelTime)
                .OnComplete(() =>
                {
                    _moving = false;
                });
            
            
        }
    }

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