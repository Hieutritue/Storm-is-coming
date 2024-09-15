﻿using System;
using UnityEngine;

public class MilitaryTile : Tile
{
    public override void Produce()
    {
        base.Produce();
        
        GameManager.Instance.UnitManager.AllySpawner.Spawn((AllyType)level);
    }

    public override bool EnoughResourceToWork()
    {
        return base.EnoughResourceToWork() && !GameManager.Instance.UnitManager.ReachedMaxCapacity;
    }
}
