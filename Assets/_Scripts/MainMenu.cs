using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public Image image;

    public void PlayGame()
    {
        image.raycastTarget = true;
        image.DOFade(1, 1).OnComplete(() => {
            SceneManager.LoadScene("InGame");
        });
    }

    public void Settings()
    {
        Debug.Log("Settings");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
