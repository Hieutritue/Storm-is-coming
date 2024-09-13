using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "TileConfig", menuName = "Config/TileConfig")]
public class TileConfig : ScriptableObject
{

}

[Serializable]
public class TileData
{
    public string name;
    public Image sprite;
    public TileRequirement requirement;
    public Tile tileType;
    public GameObject prefab;
}

[Serializable]
public class TileRequirement
{
    public int meat;
    public int wood;
    public int iron;
    public int gold;
}