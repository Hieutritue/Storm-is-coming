using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : BaselineManager, IDropHandler
{
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
