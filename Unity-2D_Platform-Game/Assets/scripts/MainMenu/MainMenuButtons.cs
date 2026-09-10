using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    AudioManagerMainMenu audioManager; // Ses yneticisi referans
    public RectTransform settingsPanel; // Settings panelinin RectTransform referans
    public GameObject mainMenuButtons; // Ana mendeki tularn GameObject grubu
    public Vector2 targetPosition = new Vector2(0, 0); // Vị trí hiện giữa màn hình
    public Vector2 previousPosition = new Vector2(-1420, 0); // Vị trí giấu bảng ngoài màn hình
    public float slideDuration = 0.5f; // Thời gian trượt bảng (saniye)

    private void Start()
    {
        // Tự động đẩy bảng Settings ra ngoài màn hình ngay khi vừa vào game
        if (settingsPanel != null)
        {
            settingsPanel.anchoredPosition = previousPosition;
        }

        GameObject audioObj = GameObject.FindGameObjectWithTag("Audio");
        if (audioObj != null)
        {
            audioManager = audioObj.GetComponent<AudioManagerMainMenu>();
        }
    }

    // --- HÀM 1: DÙNG CHO NÚT CÀI ĐẶT (GÓC MÀN HÌNH) ĐỂ MỞ BẢNG SETTINGS ---
    public void OpenSettingsFromInGame()
    {
        if (settingsPanel != null)
        {
            StopAllCoroutines();
            StartCoroutine(SlidePanelToTarget(targetPosition));
        }
        if (audioManager != null) audioManager.playButtonTouch();
    }

    // --- HÀM 2: DÙNG CHO NÚT HOME TRONG BẢNG SETTINGS ---
    public void GoToHome()
    {
        if (audioManager != null)
        {
            audioManager.playButtonTouch();
        }
        // Chuyển về Scene màn hình chính
        SceneManager.LoadScene("MainMenu"); 
    }

    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("lvl1");
        if (audioManager != null) audioManager.playButtonTouch();
    }

    public void OnSettingsButtonPressed()
    {
        Debug.Log("Settings menu alyor...");
        StartCoroutine(HideMainMenuButtonsWithDelay());
        StartCoroutine(SlidePanelToTarget(targetPosition));
        if (audioManager != null) audioManager.playButtonTouch();
    }

    public void OnCloseSettingsButtonPressed()
    {
        Debug.Log("Settings menu kapanyor...");
        StartCoroutine(SlidePanelToTargetAndShowMainMenu(previousPosition));
        if (audioManager != null) audioManager.playButtonTouch();
    }

    public void OnExitButtonPressed()
    {
        Debug.Log("Oyun kapatlyor...");
        if (audioManager != null) audioManager.playButtonTouch();
        Application.Quit();
    }

    private IEnumerator HideMainMenuButtonsWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // Yarm saniye bekle
        if (mainMenuButtons != null) mainMenuButtons.SetActive(false); // Ana men tularn gizle
    }

    private IEnumerator SlidePanelToTarget(Vector2 destination)
    {
        if (settingsPanel == null) yield break;

        Vector2 startPosition = settingsPanel.anchoredPosition; // Panelin balang pozisyonu
        float elapsedTime = 0f;

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / slideDuration;
            settingsPanel.anchoredPosition = Vector2.Lerp(startPosition, destination, t); // Pozisyonu yavaa deitir
            yield return null;
        }

        settingsPanel.anchoredPosition = destination; // Hedef pozisyona tam olarak yerletir
    }

    private IEnumerator SlidePanelToTargetAndShowMainMenu(Vector2 destination)
    {
        yield return SlidePanelToTarget(destination); // Paneli hedef pozisyona kaydr
        if (mainMenuButtons != null) mainMenuButtons.SetActive(true); // Ana men tularn tekrar gster
    }
}       