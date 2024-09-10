using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UnitManager UnitManager;
    
    public static GameManager Instance;
    
    private void Awake()
    {
        if(Instance != null) Destroy(Instance);
        Instance = this;
    }
}