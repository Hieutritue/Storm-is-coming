using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using System;
using UnityEngine.Serialization;
using Random = System.Random;

public class AllySpawner : MonoBehaviour
{
    [SerializeField] private List<BaseUnit> _units;
    
    public AllyType TestAllyType;

    private float _spawnOffset = 2;
    
    [Button]
    public void TestSpawn()
    {
        Random random = new Random();
        var xrNumber = random.NextDouble() * _spawnOffset/2 + 1f;
        var yrNumber = random.NextDouble() * _spawnOffset;
        var spawnedUnit = Instantiate(_units[(int)TestAllyType], transform);

        spawnedUnit.InitialPos = new Vector2(
            transform.position.x + (float)xrNumber,
            transform.position.y + (float)yrNumber
        );
    }

    public void Spawn(AllyType allyType)
    {
        Random random = new Random();
        var xrNumber = random.NextDouble() * _spawnOffset;
        var yrNumber = random.NextDouble() * _spawnOffset;
        var spawnedUnit = Instantiate(_units[(int)allyType], transform);

        spawnedUnit.InitialPos = new Vector2(
            transform.position.x + (float)xrNumber,
            transform.position.y + (float)yrNumber
        );
    }
}

public enum AllyType
{
    Pawn,
    Archer,
    Warrior
}
