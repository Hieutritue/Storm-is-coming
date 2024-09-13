using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GeneratorTile : Tile
{
    public void Work() 
    {
        var rm = GameManager.Instance.ResourceManager;
        switch (tileType)
        {
            case TileType.Lumberjack:
                rm.Wood += output;
                break;
            case TileType.SheepFarm:
                rm.Meat += output;
                break;
            case TileType.Forger:
                rm.Iron += output;
                break;
            case TileType.Smelter:
                rm.Gold += output;
                break;
        }
    }
}