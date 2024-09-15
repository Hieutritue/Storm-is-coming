using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UnitManager UnitManager;
    public AudioManager AudioManager;
    public ResourceManager ResourceManager;
    public UIManager UIManager;
    public GameTileManager GameTileManager;
    public TimeLineManager TimeLineManager;
    
    [SerializeField] private Texture2D _cursorTexture;
    
    public static GameManager Instance;
    
    
    
    private void Awake()
    {
        if(Instance != null) Destroy(Instance);
        Instance = this;
        
        //Cursor.SetCursor(_cursorTexture,Vector2.zero, CursorMode.Auto);
    }

    private void Start()
    {
        ResourceManager.Wood = 5;
    }
}