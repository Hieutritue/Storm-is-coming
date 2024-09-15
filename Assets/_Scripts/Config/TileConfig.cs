using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "TileConfig", menuName = "Config/TileConfig")]
public class TileConfig : ScriptableObject
{
    public GameObject basicPrefab;
    public SerializedDictionary<TileType,TileData> tileData;
}

[Serializable]
public class TileData
{
    public string name;
    public TileRequirement requirement;
    public WorkRequirement workRequirement;
    public Tile tileType;
    public int maxLevel;
    public List<Sprite> sprites;
    public List<int> output;
}

[Serializable]
public class TileRequirement
{
    public int meat;
    public int wood;
    public int gold;
}

[Serializable]
public class WorkRequirement
{
    public int meat;
    public int wood;
    public int iron;
    public int gold;
}