using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tukurukhasar : MonoBehaviour
{
    public GameObject destructionEffect; // Assign the effect prefab in the Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            healthControl healthControl = collision.GetComponent<healthControl>();
            if (healthControl != null)
            {
                healthControl.takeDamege(10);
            }
            GameObject effect = Instantiate(destructionEffect, transform.position, Quaternion.identity); // Instantiate effect
            Destroy(effect, 0.5f); // Destroy effect after 0.5 second
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground") || collision.CompareTag("Ceiling") || collision.CompareTag("Wall"))
        {
            GameObject effect = Instantiate(destructionEffect, transform.position, Quaternion.identity); // Instantiate effect
            Destroy(effect, 1f); // Destroy effect after 1 second
            Destroy(gameObject);
        }
    }
}
