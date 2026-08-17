using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField]
    Transform pointA;
    [SerializeField]
    Transform pointB;

    [SerializeField]
    float speed = 5f;

    Transform target;
    float shootTimer;
    float shootInterval = 1.5f;
    [SerializeField]
    GameObject bulletPrefab;
    bool canShoot = true;

    void Start()
    {
        target = pointB;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            if (target == pointA)
            {
                target = pointB;
                transform.Rotate(0f, 0f, 0f);
            }
            else
            {
                target = pointA;
                transform.Rotate(0f, 180f, 0f);

            }
        }
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            canShoot = true;
        }
        if (canShoot)
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
            canShoot = false;
        }

    }
}
