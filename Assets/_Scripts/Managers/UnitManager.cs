using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [ReadOnly]
    public List<BaseUnit> AllUnits = new ();
    public List<BaseUnit> AllEnemies => AllUnits.Where(u => !u.IsAlly).ToList();
    public List<BaseUnit> AllAllies => AllUnits.Where(u => u.IsAlly).ToList();
    public int Capacity { get; set; }

    public AllySpawner AllySpawner;

    [HideInInspector] public List<Tile> Houses = new ();

    private void Update()
    {
        Capacity = 4 + Houses.Sum(h => (int) Math.Pow(2,h.level+1));
    }

    public bool ReachedMaxCapacity => (AllAllies.Count - 1) >= Capacity;
}
