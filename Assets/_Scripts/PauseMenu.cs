using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused = false;
    private bool isKeyDown = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isKeyDown)
        {
            if (isPaused)
            {
                Time.timeScale = 1;
                pauseMenuUI.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                pauseMenuUI.SetActive(true);
            }

            isPaused = !isPaused;
            isKeyDown = true;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isKeyDown = false;
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
