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
    [SerializeField] private TMP_Text _tileInfo;
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

    public void Forest()
    {
        SetInfo("Forest:\n- Cost 5 wood to build\n- Produce wood for building\n- Increase production when level up\n- Max level: 3");
    }
    public void Sheep()
    {
        SetInfo("Sheep Farm:\n- Cost 5 wood to build\n- Produce meat for giving birth :v\n- Increase production when level up\n- Max level: 3");
    }
    public void Gold()
    {
        SetInfo("Gold Mine:\n- Cost 5 wood to build\n- Produce gold for buying goods\n- Increase production when level up\n- Max level: 3");
    }
    public void House()
    {
        SetInfo("House:\n- Cost 5 wood to build\n- Increase army capacity\n- Max level: 3\n(Your troops can't be born if reached max capacity)");
    }
    public void Barrack()
    {
        SetInfo("Barrack:\n- Cost 5 wood to build\n- Spawn troops\n- Lv1: 10 meat, 5 gold for NAKED GUY\n- Lv2: 3 wood, 10 meat, 7 gold for ARCHER\n- Lv3: 20 meat, 10 gold for KNIGHT\n- Lv4: 1 dude of each type and some fee for THE DEMON");
    }
    public void Thunder()
    {
        SetInfo("Call of Thunder:\n- Cost 5 wood to use\n- It goEs KABOOOMMM!!!!");
    }

    public void SetInfo(string info)
    {
        _tileInfo.text = info;
    }
}
