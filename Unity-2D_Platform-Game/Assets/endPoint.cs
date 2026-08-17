using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endPoint : MonoBehaviour
{
    [Header("Main Menu Scene Name")]
    public string mainMenuSceneName = "MainMenu"; // Ana menü sahnesinin adý

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Çarpýþan nesnenin tag'ini kontrol et
        if (collision.gameObject.tag.Equals("Player"))
        {
            // Ana menü sahnesine geçiþ yap
            SceneManager.LoadScene(0);
        }
    }
}
