using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeLineManager : MonoBehaviour
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
        if (!_eventWeeks.Any()) return; //TODO: win

        if (CurrentWeek == _eventWeeks[0].Week)
        {
            CallStorm(_eventWeeks[0].Storm);
            CallEnemyWave(_eventWeeks[0].EnemyWave);

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

    public void CallStorm(string storm)
    {
        var gtm = GameManager.Instance.GameTileManager;
        foreach (var eventChar in storm)
        {
            switch (eventChar)
            {
                case 'u':
                    gtm.ShiftByWind(new Vector2Int(0, -1));
                    break;
                case 'd':
                    gtm.ShiftByWind(new Vector2Int(0, 1));
                    break;
                case 'l':
                    gtm.ShiftByWind(new Vector2Int(-1, 0));
                    break;
                case 'r':
                    gtm.ShiftByWind(new Vector2Int(1, 0));
                    break;
                case 'z': break;
                default:
                    break;
            }
        }
    }

    public void CallEnemyWave(EnemyWave enemyWave)
    {
        _enemyWaveSpawner.SpawnWave(enemyWave);
    }
}

[Serializable]
public class EventOfWeek
{
    public int Week;
    public string Storm;
    public EnemyWave EnemyWave;
}