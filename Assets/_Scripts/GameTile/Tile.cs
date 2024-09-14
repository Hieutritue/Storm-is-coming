using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class Tile : MonoBehaviour
{
    public Pos Pos;
    public Slot OccupiedSlot;

    public Image image;
    [HideInInspector] public Transform parentAfterDrag;

    [HideInInspector] public bool isTemporary = false;

    // public List<Tile> surroundingHouses = new();
    public int level;
    public TileType tileType;
    public int MaxLevel;
    public float[] ProductPerDay;
    [SerializeField] private ProductCost[] _productCost;

    private float _timer = 0;

    bool EnoughResourceToWork()
    {
        var cost = _productCost[level];
        var rm = GameManager.Instance.ResourceManager;
        var um = GameManager.Instance.UnitManager;
        return cost.GoldCost <= rm.Gold
               && cost.IronCost <= rm.Iron
               && cost.MeatCost <= rm.Meat
               && cost.WoodCost <= rm.Wood
               && cost.PawnCost <= um.AllAllies.Count(a => a.AllyType == AllyType.Pawn)
               && cost.ArcherCost <= um.AllAllies.Count(a => a.AllyType == AllyType.Archer)
               && cost.KnightCost <= um.AllAllies.Count(a => a.AllyType == AllyType.Warrior);
    }

    private void Update()
    {
        if (!EnoughResourceToWork()) return;

        _timer += Time.deltaTime;
        if (_timer >= TimeLineManager.SecondsPerGameDay / ProductPerDay[level])
            Produce();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public virtual void Produce()
    {
        var cost = _productCost[level];
        var rm = GameManager.Instance.ResourceManager;
        var um = GameManager.Instance.UnitManager;
        rm.Gold -= cost.GoldCost;
        rm.Iron -= cost.IronCost;
        rm.Meat -= cost.MeatCost;
        rm.Wood -= cost.WoodCost;

        if (cost.PawnCost > 0
            && cost.ArcherCost > 0
            && cost.KnightCost > 0)
        {
            um.AllAllies.FirstOrDefault(a => a.AllyType == AllyType.Pawn)?.TakeDamage(2000);
            um.AllAllies.FirstOrDefault(a => a.AllyType == AllyType.Archer)?.TakeDamage(2000);
            um.AllAllies.FirstOrDefault(a => a.AllyType == AllyType.Warrior)?.TakeDamage(2000);
        }
    }

    public void SetSlot(Slot newSlot)
    {
        Pos = newSlot.Pos;
        OccupiedSlot.CurrentTile = null;
        OccupiedSlot = newSlot;
        newSlot.CurrentTile = this;
    }

    public bool CanMergeWith(Tile newTile)
    {
        return level == newTile.level
               && tileType == newTile.tileType
               && level < MaxLevel;
    }

    public void SetRaycast()
    {
        image.raycastTarget = !image.raycastTarget;
    }

//     public void CheckSurrounding()
//     {
//         // Define offsets for all 8 directions
//         Vector2Int[] directions = new Vector2Int[]
//         {
//             new(-1, -1), // bottom left
//             new(0, -1), // bottom
//             new(1, -1), // bottom right
//             new(-1, 0), // left
//             new(1, 0), // right
//             new(-1, 1), // top left
//             new(0, 1), // top
//             new(1, 1) // top right
//         };
//
//         // Get the position of this tile in the grid
//         Vector2Int thisPosGrid = gameManager.GetPosition(this, false);
//         Vector2Int thisPos = gameManager.GetPosition(this, true);
//
//         // Clear the surroundingHouses list
//         surroundingHouses.Clear();
//
//         // For each direction, get the neighboring tile
//         foreach (Vector2Int dir in directions)
//         {
//             Vector2Int neighborPos = thisPos + dir;
//
//             // Check if the neighboring position is within the grid and both coordinates are non-negative
//             if (CheckBoundary(neighborPos.x , neighborPos.y) && gameManager.tileDictionary.ContainsKey(neighborPos))
//             {
//                 // Get the neighboring tile
//                 Tile neighborTile = gameManager.tileDictionary[neighborPos];
//
//                 // Add the neighboring tile to the surroundingHouses list
//                 surroundingHouses.Add(neighborTile);
//             }
//         }
//     }
//
//     private bool CheckBoundary(int x, int y) 
//     {
//         return x >= 0 && y >= 0 && x < 4 && y < 4;
//     }
//
    public void Upgrade()
    {
        Debug.Log($"Upgraded: {gameObject.name}");
        level++;
    }
}

[Serializable]
public enum TileType
{
    Wood,
    Meat,
    Iron,
    Gold,
    House,
    Barrack,
    WeatherMachine
}

[Serializable]
public class ProductCost
{
    public int WoodCost;
    public int MeatCost;
    public int IronCost;
    public int GoldCost;
    public int PawnCost;
    public int ArcherCost;
    public int KnightCost;
}