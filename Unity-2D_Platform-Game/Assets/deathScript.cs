using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathScript : MonoBehaviour
{

    public GameObject deathmenu;
    public GameObject pauseButton;

    public void ShowDeathMenu()
    {
        // Ölüm menüsünü göster
        deathmenu.SetActive(true);
        // Oyuncunun hareketini durdur
        pauseButton.SetActive(false);
    }

}
