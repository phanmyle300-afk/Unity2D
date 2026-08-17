using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaretTopuControl : MonoBehaviour
{

private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        healthControl healthControl = collision.GetComponent<healthControl>();
        if (healthControl != null)
            {
            healthControl.takeDamege(10);
            }
        // Destroy the cannonball when it hits the player
        Destroy(gameObject);
    }
    if(collision.CompareTag("Ground") || collision.CompareTag("Ceiling") || collision.CompareTag("Wall"))
    {
        // Destroy the cannonball when it hits the ground, ceiling, or wall
        Destroy(gameObject);
    }
}
}
