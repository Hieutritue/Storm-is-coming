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
    [SerializeField] private UIData _uiData;
    [SerializeField] private string _description;
    
    private Image _image;
    

    private void Start()
    {
        _image = GetComponent<Image>();
        // tilePrefab = gameManager.tileConfig.basicPrefab;
    }

    private void Update()
    {
        if (GameManager.Instance.ResourceManager.Wood < _cost || GameManager.Instance.GameTileManager.Moving)
        {
            _image.color = Color.grey;
        }
        else
        {
            _image.color = Color.white;
        }
    }

    public void OnMouseEnter()
    {
        _uiData.SetInfo(_description);
    }

    private void OnMouseExit()
    {
        _uiData.SetInfo("");
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
        if (GameManager.Instance.GameTileManager.Moving || GameManager.Instance.ResourceManager.Wood < _cost)
        {
            GameManager.Instance.AudioManager.PlayClip(ClipName.NotEnough);
            return;
        }

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
                GameManager.Instance.GameTileManager.Tiles.Remove(slotToZap.CurrentTile);
                slotToZap.CurrentTile = null;
                Destroy(slotToZap.transform.GetChild(0).gameObject);
            }

            return;
        }


        var list = GameManager.Instance.GameTileManager.Slots.Where(s => s.CurrentTile == null).ToList();
        if (list.Count == 0) return;
        
        GameManager.Instance.AudioManager.PlayClip(ClipName.Build);
        
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