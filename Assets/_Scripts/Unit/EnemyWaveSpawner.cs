using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    [SerializeField] private EnemyWave _enemyWave;

    [Button]
    public void TestSpawnWave()
    {
        GameManager.Instance.AudioManager.PlayClip(ClipName.Trumpet);
        if (!_enemyWave) return;
        var _spawnedWave = Instantiate(_enemyWave, transform);
    }

    public void SpawnWave(EnemyWave enemyWave)
    {
        GameManager.Instance.AudioManager.PlayClip(ClipName.Trumpet);
        if (!enemyWave) return;
        Instantiate(enemyWave, transform);
    }
}