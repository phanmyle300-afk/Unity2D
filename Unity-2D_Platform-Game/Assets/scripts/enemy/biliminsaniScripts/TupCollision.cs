using UnityEngine;

public class TupCollision : MonoBehaviour
{
    public GameObject zehirPrefab; // Assign in Inspector

    private void OnCollisionEnter2D(Collision2D collision)
    {
            if (collision.collider.CompareTag("Ground")) // Ensure there are contacts
            {
                if (zehirPrefab != null)
                {
                    GameObject zehirInstance = Instantiate(zehirPrefab, collision.GetContact(0).point, Quaternion.identity);
                    Destroy(zehirInstance, 3f); // Destroy the instantiated zehir after 3 seconds
                }
                Destroy(gameObject); // Destroy the tup after spawning zehir
            }
    }

}