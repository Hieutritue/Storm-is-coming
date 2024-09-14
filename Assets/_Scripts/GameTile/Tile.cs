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
    
    [Header("Info for each level")]
    public float[] ProductPerDay;
    [SerializeField] private Sprite[] _sprites;
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

    public void Upgrade()
    {
        Debug.Log($"Upgraded: {gameObject.name}");
        
        level++;

        image.sprite = _sprites[level];
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