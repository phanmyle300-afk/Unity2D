using UnityEngine;
using TMPro;

public class MainMenuDisplay : MonoBehaviour
{
    public TMP_Text usernameText;

    private void Start()
    {
        UpdateName();
    }

    private void OnEnable()
    {
        UpdateName();
    }

    public void UpdateName()
    {
        if (usernameText == null)
        {
            usernameText = GetComponentInChildren<TMP_Text>();
        }

        if (usernameText != null)
        {
            // Lấy tên đã lưu
            string savedName = PlayerPrefs.GetString("LoggedInUsername", "");

            // Nếu lấy được tên hợp lệ từ Login thì mới đổi, nếu rỗng thì giữ nguyên tên đang gõ trên Editor!
            if (!string.IsNullOrEmpty(savedName) && !string.IsNullOrWhiteSpace(savedName))
            {
                usernameText.text = "Xin chào, " + savedName + "!";
            }

            usernameText.color = Color.white;
            usernameText.gameObject.SetActive(true);
        }
    }
}