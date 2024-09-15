using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CastleHp : MonoBehaviour
{
    [SerializeField] private BaseUnit _castle;
    private TMP_Text _tmpText;

    private void Start()
    {
        _tmpText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _tmpText.text = _castle.Health + "";
    }
}
