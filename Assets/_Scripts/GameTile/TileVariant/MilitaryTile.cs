using System;
using UnityEngine;

public class MilitaryTile : Tile
{
    public override void Produce()
    {
        base.Produce();
        
        GameManager.Instance.UnitManager.AllySpawner.Spawn((AllyType)level);
    }
}
