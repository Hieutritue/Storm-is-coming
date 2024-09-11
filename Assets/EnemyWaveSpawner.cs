using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    [SerializeField] private List<EnemyWave> _enemyWaves;

    [Button]
    public void SpawnWave()
    {
        if(!_enemyWaves.Any()) return;
        var _spawnedWave = Instantiate(_enemyWaves[0], transform);
        _enemyWaves.RemoveAt(0);
    }
}