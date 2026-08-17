using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;

    private string baseURL = "http://localhost/unity_api/";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator Login(string email, string password)
    {
        WWWForm form = new WWWForm();

        form.AddField("email", email);
        form.AddField("password", password);

        using (UnityWebRequest request =
            UnityWebRequest.Post(
                baseURL + "login.php",
                form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("LOGIN RESPONSE:");
                Debug.Log(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("LOGIN ERROR:");
                Debug.LogError(request.error);
            }
        }
    }
}