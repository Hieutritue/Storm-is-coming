using System;
using System.Collections;
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

    public float SecondsPerGameDay;
    [SerializeField] private float _delayOfStormEvents;
    [FormerlySerializedAs("_eventWeeks")] public List<EventOfWeek> EventWeeks;

    [SerializeField] private EnemyWaveSpawner _enemyWaveSpawner;

    private float _timer;
    private bool _timeStopped;

    private void Start()
    {
        EventWeeks = EventWeeks.OrderBy(e => e.Week).ToList();
    }

    private void Update()
    {
        if(_timeStopped) return;

        if (!EventWeeks.Any())
        {
            if (_enemyWaveSpawner.transform.childCount == 0)
            {
                GameManager.Instance.Win();
            }
            return;
        }

        if (CurrentWeek == EventWeeks[0].Week)
        {
            CallEnemyWave(EventWeeks[0].EnemyWave);

            EventWeeks.RemoveAt(0);
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

    public IEnumerator CallStorm(string storm)
    {
        _timeStopped = true;
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

            yield return new WaitForSeconds(_delayOfStormEvents);
        }

        _timeStopped = false;
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
    public EnemyWave EnemyWave;
}