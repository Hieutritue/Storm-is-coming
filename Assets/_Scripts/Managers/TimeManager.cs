using System;
using NaughtyAttributes;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [ReadOnly] public int CurrentWeek = 0;
    [ReadOnly] public int CurrentDay = 1;

    [SerializeField] private float _secondsPerGameDay;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _secondsPerGameDay)
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