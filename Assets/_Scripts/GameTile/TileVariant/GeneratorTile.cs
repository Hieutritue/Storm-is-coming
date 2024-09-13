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
        switch (tileType)
        {
            case TileType.Lumberjack:
                resourceManager.Wood += output;
                break;
            case TileType.SheepFarm:
                resourceManager.Meat += output;
                break;
            case TileType.Forger:
                resourceManager.Iron += output;
                break;
            case TileType.Smelter:
                resourceManager.Gold += output;
                break;
        }
    }
}