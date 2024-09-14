using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeManager : MonoBehaviour
{
    [ReadOnly] public int CurrentWeek = 1;
    [ReadOnly] public int CurrentDay = 1;

    public static float SecondsPerGameDay;
    [SerializeField] private List<EventOfWeek> _eventWeeks;

    [SerializeField] private EnemyWaveSpawner _enemyWaveSpawner;

    private float _timer;

    private void Start()
    {
        _eventWeeks = _eventWeeks.OrderBy(e => e.Week).ToList();
    }

    private void Update()
    {
        if(!_eventWeeks.Any()) return; //TODO: win
        
        if (CurrentWeek == _eventWeeks[0].Week)
        {
            _eventWeeks[0].CallStorm();
            _enemyWaveSpawner.SpawnWave(_eventWeeks[0].EnemyWave);
            
            _eventWeeks.RemoveAt(0);
        }
        
        _timer += Time.deltaTime;
        if (_timer >= SecondsPerGameDay)
        {
            CurrentDay++;
            _timer = 0;

            if (CurrentDay == 8)
            {
                CurrentDay = 1;
                CurrentWeek++;
            }
        }
    }
    
}

[Serializable]
public class EventOfWeek
{
    public int Week;
    public string Storm;
    public EnemyWave EnemyWave;

    public void CallStorm()
    {
        // throw new NotImplementedException();
    }
}