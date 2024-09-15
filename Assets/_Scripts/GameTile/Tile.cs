using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using DG.Tweening;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    public Pos Pos;
    public Slot OccupiedSlot;

    public Image Image;
    [HideInInspector] public Transform parentAfterDrag;

    [HideInInspector] public bool isTemporary = false;

    // public List<Tile> surroundingHouses = new();
    public int level;
    public TileType tileType;
    public int MaxLevel;

    [Header("Info for each level")] public float[] ProductPerDay;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private ProductCost[] _productCost;

    [FormerlySerializedAs("_progressBar")] [HideInInspector]
    public Slider ProgressBar;

    private float _timer = 0;

    private void Awake()
    {
        Image = GetComponent<Image>();
        ProgressBar = GetComponentInChildren<Slider>();
        if(ProgressBar) ProgressBar.value = 0;
    }

    public virtual bool EnoughResourceToWork()
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

    private void OnDisable()
    {
        if(!(this is GeneratorTile || this is MilitaryTile) )
            GameManager.Instance.UnitManager.Houses.Remove(this);
    }

    private void Update()
    {
        if (isTemporary)
        {
            return;
        }

        if (!EnoughResourceToWork()) return;
        _timer += Time.deltaTime;

        if (ProgressBar)
            ProgressBar.value =
                _timer / (GameManager.Instance.TimeLineManager.SecondsPerGameDay / ProductPerDay[level]);

        if (_timer >= GameManager.Instance.TimeLineManager.SecondsPerGameDay / ProductPerDay[level])
        {
            _timer = 0;
            Produce();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public virtual void Produce()
    {
        ProgressBar.value = 0;
        Image.DOColor(Color.yellow, .1f).OnComplete(() => { Image.DOColor(Color.white, 0.1f); });

        Debug.Log("Produce");

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

    public void ChangeTransparency(float a)
    {
        var color = Image.color;
        color.a = a;
        Image.color = color;
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
        Image.raycastTarget = !Image.raycastTarget;
    }

    public void Upgrade()
    {
        Debug.Log($"Upgraded: {gameObject.name}");

        level++;

        transform.DOScale(Vector3.one * 1.2f,0.1f)
            .OnComplete(()=>transform.DOScale(Vector3.one * 0.8f,.1f));

        Image.sprite = _sprites[level];
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