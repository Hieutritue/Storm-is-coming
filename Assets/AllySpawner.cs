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
    
    public int UnitIndex;

    private float _spawnOffset = 2;
    
    [Button]
    public void Spawn()
    {
        Random random = new Random();
        var xrNumber = random.NextDouble() * _spawnOffset;
        var yrNumber = random.NextDouble() * _spawnOffset;
        var spawnedUnit = Instantiate(_units[UnitIndex], transform);

        spawnedUnit.InitialPos = new Vector2(
            transform.position.x + (float)xrNumber,
            transform.position.y + (float)yrNumber
        );
    }
    
}
