using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIData : MonoBehaviour
{
    [SerializeField] private TMP_Text _wood;
    [SerializeField] private TMP_Text _meat;
    [SerializeField] private TMP_Text _gold;
    [SerializeField] private TMP_Text _men;
    [SerializeField] private TMP_Text _week;
    [SerializeField] private TMP_Text _day;
    [SerializeField] private TMP_Text _nextWave;
    private void Update()
    {
        _wood.text = GameManager.Instance.ResourceManager.Wood.ToString();
        _meat.text = GameManager.Instance.ResourceManager.Meat.ToString();
        _gold.text = GameManager.Instance.ResourceManager.Gold.ToString();
        _men.text = (GameManager.Instance.UnitManager.AllAllies.Count - 1) + "/" + GameManager.Instance.UnitManager.Capacity;
        _week.text = "Week: " + GameManager.Instance.TimeLineManager.CurrentWeek;
        _day.text = "Day: " + GameManager.Instance.TimeLineManager.CurrentDay;
        if(GameManager.Instance.TimeLineManager.EventWeeks.Count==0) return;
        _nextWave.text = "Next wave in " + (GameManager.Instance.TimeLineManager.EventWeeks[0].Week - GameManager.Instance.TimeLineManager.CurrentWeek) + " weeks";
    }
}
