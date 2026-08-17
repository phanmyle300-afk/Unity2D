using UnityEngine;

public class Zehir : MonoBehaviour
{
    private Collider2D other;
    private bool isEntered = false;
    private float damageTimer = 1f; // Timer to track damage intervals

    private void Update()
    {
        if (isEntered)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= 1f) // Check if 1 second has passed
            {
                damageTimer = 0f; // Reset the timer
                if (other != null && other.CompareTag("Player"))
                {
                    healthControl healthControl = other.GetComponent<healthControl>();
                    if (healthControl != null)
                    {
                        healthControl.takeDamege(10); // Damage the player
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isEntered = true;
            this.other = other;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isEntered = false;
            this.other = null;
        }
    }
}
