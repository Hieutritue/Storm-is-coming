using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : BaselineManager, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public bool isTemporary = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isTemporary)
        {
            gameManager.currentHoldingTile = gameObject;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            SetRaycast();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isTemporary)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isTemporary)
        {
            gameManager.currentHoldingTile = null;
            transform.SetParent(parentAfterDrag);
            SetRaycast();
        }
    }

    public void SetRaycast()
    {
        image.raycastTarget = !image.raycastTarget;
    }

    public void CheckSurrounding()
    {
        // Implementation for checking surrounding tiles
    }
}
