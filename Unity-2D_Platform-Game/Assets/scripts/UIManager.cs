using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject pauseMenuPanel;
    public GameObject victoryPanel;
    public GameObject gameOverPanel;

    [Header("Level Buttons (Nút chọn màn)")]
    public Button[] levelButtons;

    private void Start()
    {
        // 1. Cập nhật trạng thái mở khóa các level
        UpdateLevelButtons();

        // 2. Mặc định đảm bảo thời gian game chạy bình thường
        Time.timeScale = 1f;
    }

    // --- MỞ KHÓA LEVEL ---
    public void UpdateLevelButtons()
    {
        // Lấy level cao nhất đã mở khóa (Mặc định là Level 1)
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 <= unlockedLevel)
            {
                levelButtons[i].interactable = true; // Cho phép bấm
            }
            else
            {
                levelButtons[i].interactable = false; // Khóa nút lại
            }
        }
    }

    // --- ĐIỀU HƯỚNG MAIN MENU ---
    public void OpenLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        levelSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void LoadLevel(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    // --- PAUSE MENU (TẠM DỪNG GAME) ---
    public void TogglePause()
    {
        if (pauseMenuPanel == null) return;

        bool isPaused = !pauseMenuPanel.activeSelf;
        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f; // Ngưng thời gian khi Pause
    }

    // --- KẾT THÚC MÀN CHƠI (THẮNG/THUA) ---
    public void ShowVictory(int currentLevelIndex)
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);
        Time.timeScale = 0f;

        // Lưu mở khóa level tiếp theo
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (currentLevelIndex + 1 > unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevelIndex + 1);
            PlayerPrefs.Save();
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartCurrentLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}