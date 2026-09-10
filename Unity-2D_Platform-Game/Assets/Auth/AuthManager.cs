using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore; // Lưu thêm tên vào Database (nếu dùng)
using TMPro;

public class AuthManager : MonoBehaviour
{
    [Header("Giao diện 2 Trang (Panels)")]
    public GameObject loginPanel;
    public GameObject registerPanel;

    [Header("Trang Đăng Nhập")]
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;
    public TextMeshProUGUI loginStatusText;

    [Header("Trang Đăng Ký")]
    public TMP_InputField regUsernameInput; // Ô Tên người dùng
    public TMP_InputField regEmailInput;
    public TMP_InputField regPasswordInput;
    public TMP_InputField regConfirmPasswordInput; // Ô Nhập lại mật khẩu
    public TextMeshProUGUI regStatusText;

    [Header("Chuyển Scene Game")]
    public string gameSceneName = "Lvl1";

    private FirebaseAuth auth;

    void Start()
    {
        OpenLoginPanel();

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase Auth đã sẵn sàng!");
            }
            else
            {
                Debug.LogError("Lỗi kết nối Firebase: " + dependencyStatus);
            }
        });
    }

    // --- HÀM CHUYỂN TRANG ---
    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        if (loginStatusText != null) loginStatusText.text = "";
    }

    public void OpenRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        if (regStatusText != null) regStatusText.text = "";
    }

    // --- XỬ LÝ ĐĂNG KÝ (Có kiểm tra Nhập lại mật khẩu & Tên) ---
    public void RegisterButton()
    {
        StartCoroutine(Register(
            regUsernameInput.text, 
            regEmailInput.text, 
            regPasswordInput.text, 
            regConfirmPasswordInput.text
        ));
    }

    private IEnumerator Register(string username, string email, string password, string confirmPassword)
    {
        // 1. Kiểm tra rỗng
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            regStatusText.text = "Vui lòng nhập đầy đủ thông tin!";
            yield break;
        }

        // 2. Kiểm tra Mật khẩu nhập lại có khớp không
        if (password != confirmPassword)
        {
            regStatusText.text = "Mật khẩu nhập lại không trùng khớp!";
            yield break;
        }

        // 3. Gửi lên Firebase
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            regStatusText.text = "Đăng ký thất bại: Mật khẩu >= 6 ký tự hoặc Email đã tồn tại!";
        }
        else
        {
            regStatusText.text = "Đăng ký thành công!";
            yield return new WaitForSeconds(1.2f);
            OpenLoginPanel(); // Thành công tự quay về Đăng nhập
        }
    }

    // --- XỬ LÝ ĐĂNG NHẬP ---
    public void LoginButton()
    {
        StartCoroutine(Login(loginEmailInput.text, loginPasswordInput.text));
    }

    private IEnumerator Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            loginStatusText.text = "Vui lòng nhập đủ Email và Mật khẩu!";
            yield break;
        }

        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            loginStatusText.text = "Sai Email hoặc Mật khẩu!";
        }
        else
        {
            loginStatusText.text = "Đăng nhập thành công!";
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(gameSceneName);
        }
    }
}