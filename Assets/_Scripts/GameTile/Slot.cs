using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : BaselineManager, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool isUnderCursor = false;

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
        GameObject droppedObject = gameManager.currentHoldingTile;
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
