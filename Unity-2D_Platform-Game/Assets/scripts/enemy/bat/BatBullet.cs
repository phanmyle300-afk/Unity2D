using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BatBullet : MonoBehaviour
{
    private GameObject player;
    public float damage = 2;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            player.GetComponent<healthControl>().takeDamege(damage);
            Destroy(gameObject);
        }
    }
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        Destroy(gameObject, 5f);
    }
}
