using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    private GameManager GM => GameManager.Instance;

    public SerializedDictionary<Resource, TextMeshProUGUI> resourceTexts;
    public List<TextMeshProUGUI> troopTexts;

    /*
    *   Add more UI Elements here using:
    *   public TextMeshProUGUI myText;
    */

    
    public void UpdateResourceTexts() {
        foreach (var resource in resourceTexts) {
            switch (resource.Key) {
                case Resource.Wood:
                    resource.Value.text = GM.ResourceManager.Wood.ToString();
                    break;
                case Resource.Meat:
                    resource.Value.text = GM.ResourceManager.Meat.ToString();
                    break;
                case Resource.Iron:
                    resource.Value.text = GM.ResourceManager.Iron.ToString();
                    break;
                case Resource.Gold:
                    resource.Value.text = GM.ResourceManager.Gold.ToString();
                    break;
            }
        }
    }
}

public enum Resource {
    Wood,
    Meat,
    Iron,
    Gold
}