using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TaretControl : MonoBehaviour
{
    public GameObject taretTopuPrefab; // Reference to the TaretTopu prefab
    public Transform firePoint; // The point from where the TaretTopu will be fired
    public float fireRate = 1f; // Fire rate in seconds
    private float nextFireTime = 0f; // Time to track the next fire
    private GameObject player;
    private Animator Taretanimator;
    private float distance;
    public float range = 20f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        Taretanimator = this.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        Distance();

        if (distance < range)
        {
            Taretanimator.SetBool("isWake", true);
            if (Time.time >= nextFireTime)
            {
                Taretanimator.SetTrigger("isShoot");
                Fire();
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            Taretanimator.SetBool("isWake", false);
        }
    }

    private void Distance()
    {
        distance = Vector2.Distance(this.gameObject.transform.position, player.transform.position);
    }

    void Fire()
    {
        GameObject taretTopu = Instantiate(taretTopuPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = taretTopu.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(firePoint.up * 500f); // Apply force to the TaretTopu in the upward direction
        }
    }
}
