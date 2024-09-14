using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileSpawner : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject tilePrefab;
    public Transform tileParent;
    private GameObject spawnedTile;
    private TileRequirement tileRequirement;
    public TileType tileType;

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
        
        spawnedTile.GetComponent<Tile>().isTemporary = true;
        spawnedTile.GetComponent<Tile>().SetRaycast();
        GameManager.Instance.GameTileManager.currentHoldingTile = spawnedTile;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (spawnedTile != null)
        {
            spawnedTile.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (spawnedTile != null)
        {
            if (spawnedTile.GetComponent<Tile>().parentAfterDrag == null)
            {
                Destroy(spawnedTile);
                return;
            }

            var tileComponent = spawnedTile.GetComponent<Tile>();
            
            tileComponent.isTemporary = false;
            tileComponent.SetRaycast();
            spawnedTile.transform.SetParent(tileComponent.parentAfterDrag);

            var slotToDropOn = tileComponent.parentAfterDrag.GetComponent<Slot>();
            slotToDropOn.CurrentTile = tileComponent;
            tileComponent.OccupiedSlot = slotToDropOn;
            tileComponent.Pos = slotToDropOn.Pos;
            GameManager.Instance.GameTileManager.Tiles.Add(tileComponent);
            
            
            GameManager.Instance.GameTileManager.currentHoldingTile = null;
            spawnedTile = null;
            // gameManager.CheckForTile();
            // gameManager.CheckAllTile();
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
