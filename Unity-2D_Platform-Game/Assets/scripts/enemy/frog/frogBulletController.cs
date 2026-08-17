using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogBulletController : MonoBehaviour
{
    [Header("Damage settings")]
    public float damage;

    private healthControl playerHealthController;

    private void Start()
    {
        playerHealthController = GameObject.FindWithTag("Player").GetComponent<healthControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            playerHealthController.takeDamege(damage);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag.Equals("Ground") || collision.gameObject.tag.Equals("Wall") || collision.gameObject.tag.Equals("Ceilling"))
        {
            Destroy(this.gameObject);
        }
    }
}
