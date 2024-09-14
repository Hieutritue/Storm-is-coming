using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class Slot : MonoBehaviour ,IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Pos Pos;
    public Tile CurrentTile;
    
    private bool isUnderCursor = false;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        var obj = GameManager.Instance.GameTileManager.currentHoldingTile;
        
        if(Input.GetMouseButtonUp(0)) _image.color = new Color(1, 1, 1, 0);
        
        if (isUnderCursor && obj)
        {
            _image.sprite = obj.Image.sprite;
            _image.color = new Color(1, 1, 1, 0.7f);
        }
        else
        {
            _image.color = new Color(1, 1, 1, 0);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            isUnderCursor = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isUnderCursor = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Tile tile = GameManager.Instance.GameTileManager.currentHoldingTile;
        
        if (!tile.isTemporary)
        {
            if (transform.childCount != 0)
            {
                var changeTile = transform.GetChild(0).GetComponent<Tile>(); ;
    
                (changeTile.parentAfterDrag, tile.parentAfterDrag) = 
                    (tile.parentAfterDrag, changeTile.parentAfterDrag);
    
                changeTile.transform.SetParent(changeTile.parentAfterDrag);
            }
    
            tile.parentAfterDrag = transform;
        }
        else
        {
            if (transform.childCount == 0)
            {
                tile.parentAfterDrag = transform;
            }
        }
    }
}
[Serializable]
public struct Pos
{
    public int X;
    public int Y;

    public Pos(int x, int y)
    {
        X = x;
        Y = y;
    }
}