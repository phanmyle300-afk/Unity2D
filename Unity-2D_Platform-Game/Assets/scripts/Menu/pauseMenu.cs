using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject _pauseButton;

    public void pauseButton()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        _pauseButton.SetActive(false);
    }

    public void resumeButton()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        _pauseButton.SetActive(true);

    }

    public void settingsButton()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }

    public void backButton()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void quitButton()
    {
        Time.timeScale = 1f;
        Application.Quit();
        Debug.Log("Oyun kapatýlýyor...");
    }

    public void restartButton()
    {
        Time.timeScale = 1f;
        sceneController.instance.RestartLevel();
    }

    public void mainMenuButton()
    {
        Time.timeScale = 1f;
        sceneController.instance.LoadMainMenu();
    }



}
