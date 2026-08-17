using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yavaslatanDal : MonoBehaviour
{
    private GameObject _player;
    private MainCharacterSlow _playerSlow; // MainCharacterSlow referansý

    public float lifeTime = 5f; // Dalýn yaþam süresi
    public float slowDuration = 2f; // Yavaþlatma süresi
    public float slowMultiplier = 0.5f; // Yavaþlatma çarpaný

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (!_player)
        {
            Debug.LogError("Player bulunamadý!");
        }

        _playerSlow = _player.GetComponent<MainCharacterSlow>();
        if (!_playerSlow)
        {
            Debug.LogError("MainCharacterSlow bileþeni bulunamadý!");
        }
    }

    private void Update()
    {
        // Dalýn yaþam süresi dolduðunda nesneyi yok et
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {

            if (_playerSlow != null)
            {
                // Yavaþlatma etkisini uygula
                _playerSlow.ApplySlowEffect(slowMultiplier, slowDuration);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {

            if (_playerSlow != null)
            {
                // Oyuncu dalýn dýþýna çýktýðýnda hýz otomatik olarak eski haline dönecek
                // Bu iþlem MainCharacterSlow tarafýndan yönetiliyor.
            }
        }
    }
}
