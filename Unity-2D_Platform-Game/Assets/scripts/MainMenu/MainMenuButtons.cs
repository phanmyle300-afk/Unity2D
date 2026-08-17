using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    AudioManagerMainMenu audioManager; // Ses yöneticisi referansý
    public RectTransform settingsPanel; // Settings panelinin RectTransform referansý
    public GameObject mainMenuButtons; // Ana menüdeki tuþlarýn GameObject grubu
    public Vector2 targetPosition = new Vector2(200, 0); // Panelin hedef pozisyonu
    public Vector2 previousPosition = new Vector2(-1420, 0); // Panelin önceki pozisyonu
    public float slideDuration = 1.0f; // Kayma süresi (saniye)

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerMainMenu>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManagerMainMenu bulunamadý!");
        }
    }
    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("lvl1");
        audioManager.playButtonTouch();
    }

    public void OnSettingsButtonPressed()
    {
        Debug.Log("Settings menu açýlýyor...");
        StartCoroutine(HideMainMenuButtonsWithDelay());
        StartCoroutine(SlidePanelToTarget(targetPosition));
        audioManager.playButtonTouch();

    }

    public void OnCloseSettingsButtonPressed()
    {
        Debug.Log("Settings menu kapanýyor...");
        StartCoroutine(SlidePanelToTargetAndShowMainMenu(previousPosition));
        audioManager.playButtonTouch();

    }

    public void OnExitButtonPressed()
    {
        Debug.Log("Oyun kapatýlýyor...");
        audioManager.playButtonTouch();
        Application.Quit();
    }

    private IEnumerator HideMainMenuButtonsWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // Yarým saniye bekle
        mainMenuButtons.SetActive(false); // Ana menü tuþlarýný gizle
    }

    private IEnumerator SlidePanelToTarget(Vector2 destination)
    {
        Vector2 startPosition = settingsPanel.anchoredPosition; // Panelin baþlangýç pozisyonu
        float elapsedTime = 0f;

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / slideDuration;
            settingsPanel.anchoredPosition = Vector2.Lerp(startPosition, destination, t); // Pozisyonu yavaþça deðiþtir
            yield return null;
        }

        settingsPanel.anchoredPosition = destination; // Hedef pozisyona tam olarak yerleþtir
    }

    private IEnumerator SlidePanelToTargetAndShowMainMenu(Vector2 destination)
    {
        yield return SlidePanelToTarget(destination); // Paneli hedef pozisyona kaydýr
        mainMenuButtons.SetActive(true); // Ana menü tuþlarýný tekrar göster
    }
}
