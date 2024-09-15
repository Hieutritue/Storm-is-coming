using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class Wind : MonoBehaviour
{
    [SerializeField] private RectTransform _windHorizontal;
    [SerializeField] private RectTransform _windVertical;

    [Button]
    public void Down()
    {
        _windVertical.DOAnchorPos(new Vector2(0, 1500), 0);
        _windVertical.DOAnchorPos(new Vector2(0, -1500), GameManager.Instance.GameTileManager.TravelTime*3);
    }

    [Button]
    public void Left()
    {
        _windHorizontal.DOAnchorPos(new Vector2(2000, 0), 0);
        _windHorizontal.DOAnchorPos(new Vector2(-2000, 0), GameManager.Instance.GameTileManager.TravelTime*3);
    }

    [Button]
    public void Up()
    {
        _windVertical.DOAnchorPos(new Vector2(0, -1500), 0);
        _windVertical.DOAnchorPos(new Vector2(0, 1500), GameManager.Instance.GameTileManager.TravelTime*3);
    }

    [Button]
    public void Right()
    {
        _windHorizontal.DOAnchorPos(new Vector2(-2000, 0), 0);
        _windHorizontal.DOAnchorPos(new Vector2(2000, 0), GameManager.Instance.GameTileManager.TravelTime*3);
    }
}