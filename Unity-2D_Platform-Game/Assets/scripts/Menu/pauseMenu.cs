using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Thêm thư viện quản lý Scene

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
        Debug.Log("Game Exiting...");
    }

    public void restartButton()
    {
        Time.timeScale = 1f;
        if (sceneController.instance != null)
        {
            sceneController.instance.RestartLevel();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void mainMenuButton()
    {
        Time.timeScale = 1f; // Khôi phục lại thời gian game trước khi chuyển Scene
        SceneManager.LoadScene("MainMenü"); // Chuyển chính xác về Scene MainMenü (số 1)
    }
}