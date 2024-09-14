using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GeneratorTile : Tile
{
    public override void Produce() 
    {
        base.Produce();
        
        var rm = GameManager.Instance.ResourceManager;
        switch (tileType)
        {
            case TileType.Wood:
                rm.Wood += 1;
                break;
            case TileType.Meat:
                rm.Meat += 1;
                break;
            case TileType.Iron:
                rm.Iron += 1;
                break;
            case TileType.Gold:
                rm.Gold += 1;
                break;
        }
    }
}