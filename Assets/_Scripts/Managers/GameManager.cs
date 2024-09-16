using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Instance != null) Destroy(Instance);
        Instance = this;

        //Cursor.SetCursor(_cursorTexture,Vector2.zero, CursorMode.Auto);
    }

    private void Start()
    {
        ResourceManager.Wood = 5;
    }

    public void Lose()
    {
        SceneManager.LoadScene("Lost");
    }

    public void Win()
    {
        SceneManager.LoadScene("Win");
    }
}