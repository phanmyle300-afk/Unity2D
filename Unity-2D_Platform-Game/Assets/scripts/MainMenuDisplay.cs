using UnityEngine;
using TMPro;

public class MainMenuDisplay : MonoBehaviour
{
    public TMP_Text usernameText;

    private void Start()
    {
        // Lấy tên đã lưu, nếu chưa có thì hiển thị "Người chơi"
        string savedUsername = PlayerPrefs.GetString("LoggedInUsername", "Người chơi");
        
        Debug.Log("Tên lấy từ PlayerPrefs là: " + savedUsername);

        if (usernameText != null)
        {
            usernameText.text = "Xin chào, " + savedUsername + "!";
        }
        else
        {
            Debug.LogError("Chưa kéo đối tượng UsernameText vào ô script!");
        }
    }
}