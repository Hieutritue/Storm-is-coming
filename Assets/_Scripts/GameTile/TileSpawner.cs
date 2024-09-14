using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileSpawner : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Tile tilePrefab;
    public Transform tileParent;
    private Tile spawnedTile;
    private TileRequirement tileRequirement;

    private void Start(){
        // tilePrefab = gameManager.tileConfig.basicPrefab;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        spawnedTile = Instantiate(tilePrefab, eventData.position, Quaternion.identity);
        spawnedTile.transform.SetParent(tileParent, false);
        
        // Set the width and height of the spawnedTile to be the same as the prefab
        RectTransform prefabRectTransform = tilePrefab.GetComponent<RectTransform>();
        RectTransform spawnedRectTransform = spawnedTile.GetComponent<RectTransform>();
        spawnedRectTransform.sizeDelta = prefabRectTransform.sizeDelta;
        
        spawnedTile.isTemporary = true;
        spawnedTile.SetRaycast();
        GameManager.Instance.GameTileManager.currentHoldingTile = spawnedTile;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (spawnedTile != null)
        {
            spawnedTile.ChangeTransparency(0.4f);
            spawnedTile.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (spawnedTile != null)
        {
            if (spawnedTile.GetComponent<Tile>().parentAfterDrag == null)
            {
                Destroy(spawnedTile.gameObject);
                return;
            }
            
            spawnedTile.ChangeTransparency(1);
            
            spawnedTile.isTemporary = false;
            spawnedTile.SetRaycast();
            spawnedTile.transform.SetParent(spawnedTile.parentAfterDrag);

            var slotToDropOn = spawnedTile.parentAfterDrag.GetComponent<Slot>();
            slotToDropOn.CurrentTile = spawnedTile;
            spawnedTile.OccupiedSlot = slotToDropOn;
            spawnedTile.Pos = slotToDropOn.Pos;
            GameManager.Instance.GameTileManager.Tiles.Add(spawnedTile);
            
            
            GameManager.Instance.GameTileManager.currentHoldingTile = null;
            spawnedTile = null;
            // GameManager.Instance.GameTileManager.CheckForTile();
            // GameManager.Instance.GameTileManager.CheckAllTile();
        }
    }

    // public void CheckForresourceAvailability()
    // {
    //     var rm = GameManager.Instance.ResourceManager;
    //
    //     if (rm.CheckEnoughResources(tileRequirement))
    //     {
    //         rm.Wood -= tileRequirement.wood;
    //         rm.Meat -= tileRequirement.meat;
    //         rm.Iron -= tileRequirement.iron;
    //         rm.Gold -= tileRequirement.gold;
    //     }
    //     else
    //     {
    //         Destroy(spawnedTile);
    //     }
    // }
}
