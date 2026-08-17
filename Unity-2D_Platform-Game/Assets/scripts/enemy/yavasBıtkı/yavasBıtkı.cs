using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yavasBıtkı : MonoBehaviour
{
    public GameObject _yavaslatanDal;
    public float detectableRange = 5f; // Algılama aralığı
    public float createTime = 2f; // Oluşturma süresi

    private GameObject _player;
    private Vector2 instantiatePosition; // Düzeltme: Türkçe karakter kullanma
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null)
        {
            Debug.LogError("Player bulunamadı!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer bulunamadı!");
        }
    }

    private void Update()
    {
        rangeDetect();
        FlipTowardsPlayer();
    }

    public void rangeDetect()
    {
        if (_player == null) return;

        if (Vector3.Distance(_player.transform.position, transform.position) < detectableRange)
        {
            instantiatePosition = new Vector2(_player.transform.position.x, transform.position.y - 0.62f);

            if (createTime > 0)
            {
                createTime -= Time.deltaTime;
            }
            else
            {
                Instantiate(_yavaslatanDal, instantiatePosition, Quaternion.identity);
                createTime = 2f;
            }
        }
    }

    private void FlipTowardsPlayer()
    {
        if (_player == null || spriteRenderer == null) return;

        bool isPlayerOnLeft = _player.transform.position.x < transform.position.x;
        spriteRenderer.flipX = isPlayerOnLeft;
    }
}
