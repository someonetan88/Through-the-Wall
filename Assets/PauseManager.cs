using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseButton;
    public GameObject pauseMenu;

    public void ShowPaused(bool paused)
    {
        if (paused) 
        {
            pauseButton.SetActive(!paused);
            pauseMenu.SetActive(paused);
            Time.timeScale = 0f;
        }else
        {
            pauseButton.SetActive(!paused);
            pauseMenu.SetActive(paused);
            Time.timeScale = 1f;
        }
    }

    public void ChangeMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
