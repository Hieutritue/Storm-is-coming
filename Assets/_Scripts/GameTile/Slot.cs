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

    private void Start()
    {
        // Debug.Log($"{gameObject.name}: {Pos.X}, {Pos.Y}");
    }

    private void Update()
    {
        if (isUnderCursor)
        {
            GetComponent<Image>().color = Color.green;
        }
        else
        {
            // Reset the slot color to its base color when the cursor is not over it
            GetComponent<Image>().color = Color.white; // Replace with your slot's base color
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
        GameObject droppedObject = GameManager.Instance.GameTileManager.currentHoldingTile;
        Tile tile = droppedObject.GetComponent<Tile>();

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