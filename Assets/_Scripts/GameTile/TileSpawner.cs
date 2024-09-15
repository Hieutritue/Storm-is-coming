using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

public class TileSpawner : MonoBehaviour
{
    public Tile tilePrefab;
    public Transform tileParent;
    private Tile spawnedTile;
    public TileRequirement tileRequirement;
    [SerializeField] private Animator _explosion;
    [SerializeField] private int _cost = 5;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        // tilePrefab = gameManager.tileConfig.basicPrefab;
    }

    private void Update()
    {
        if(tilePrefab.tileType == TileType.Thunder) return;
        
        if (GameManager.Instance.ResourceManager.Wood < _cost)
        {
            _image.color = Color.grey;
        }
        else
        {
            _image.color = Color.white;
        }
    }

    bool MeetRequirement()
    {
        var rm = GameManager.Instance.ResourceManager;
        return tileRequirement.gold <= rm.Gold
               && tileRequirement.wood <= rm.Wood
               && tileRequirement.meat <= rm.Meat;
    }

    public void SpawnOnRandomSlot()
    {
        if (tilePrefab.tileType == TileType.Thunder)
        {
            var listToZap = GameManager.Instance.GameTileManager.Slots;
            var slotToZap = listToZap[new Random().Next(0, listToZap.Count - 1)];

            var ex = Instantiate(_explosion, slotToZap.Transform);
            ex.transform.position = new Vector2(slotToZap.Transform.position.x, slotToZap.Transform.position.y - 3);
            
            GameManager.Instance.AudioManager.PlayClip(ClipName.Explosion);
            
            UniTask.Delay(600).ContinueWith(() => Destroy(ex.gameObject));
            
            if (slotToZap.CurrentTile)
            {
                slotToZap.CurrentTile = null;
                Destroy(slotToZap.transform.GetChild(0).gameObject);
            }
            return;
        }
        
        if(GameManager.Instance.ResourceManager.Wood < _cost) return;
        
        var list = GameManager.Instance.GameTileManager.Slots.Where(s => s.CurrentTile == null).ToList();
        if(list.Count == 0) return;
        var slot = list[new Random().Next(0, list.Count - 1)];
        spawnedTile = Instantiate(tilePrefab, slot.transform);
        spawnedTile.transform.localPosition = Vector3.zero;
        spawnedTile.transform.localScale = Vector3.one * 0.8f;
        
        slot.CurrentTile = spawnedTile;
        
        spawnedTile.SetSlot(slot);
        spawnedTile.ProgressBar?.gameObject.SetActive(true);

        if (!(spawnedTile is GeneratorTile || spawnedTile is MilitaryTile))
            GameManager.Instance.UnitManager.Houses.Add(spawnedTile);
        
        spawnedTile.Pos = slot.Pos;
        GameManager.Instance.GameTileManager.Tiles.Add(spawnedTile);

        GameManager.Instance.ResourceManager.Wood -= _cost;
    }

    
}