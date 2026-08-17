using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class biliminsaniControl : MonoBehaviour
{
    public GameObject tupPrefab; // Assign your 'tup' prefab in the inspector
    public Transform throwPoint; // Where the bottle will be thrown from
    public float Range = 20f;
    public float throwForce = 7f;
    public float throwAngle = 45f; // Angle for the throw
    public float throwInterval = 2f; // Interval between throws in seconds
    public bool isEntered = false;
    private Collider2D other; // Store the collider reference
    private bool canThrow = true; // Flag to control throwing
    private GameObject Player;
    private Animator animator;
    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        animator = this.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        distance();
        startAttack();
    }
   
    private void distance()
    {
        float distance = Vector2.Distance(this.gameObject.transform.position, Player.transform.position);
        if (distance < Range)
        {
            isEntered = true;
        }
        else
        {
            isEntered = false; // Reset isEntered when out of range
        }
    }

    private void startAttack()
    {
        if (isEntered)
        {
            if (canThrow)
            {
                // Rotate biliminsani to face the player
                Vector3 direction = Player.transform.position - transform.position;
                if (direction.x > 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }

                StartCoroutine(gecikmeliTupAtma());
            }
        }
    }
    private IEnumerator gecikmeliTupAtma()
    {
        canThrow = false;
        ThrowTup(Player.transform); // Use the stored collider reference
        yield return new WaitForSeconds(3f); // Wait for the specified interval
        canThrow = true;

    }


    void ThrowTup(Transform target)
    {
        animator.SetTrigger("isAttacking");
        if (tupPrefab != null && throwPoint != null)
        {
            GameObject tup = Instantiate(tupPrefab, throwPoint.position, Quaternion.identity);
            Rigidbody2D rb = tup.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 start = throwPoint.position;
                Vector2 end = target.position;
                float distance = end.x - start.x;
                float gravity = Mathf.Abs(Physics2D.gravity.y);
                float angle = throwAngle * Mathf.Deg2Rad;

                // Calculate initial velocity
                float velocity = Mathf.Sqrt(Mathf.Abs(distance) * gravity / Mathf.Sin(2 * angle));
                float vx = velocity * Mathf.Cos(angle) * Mathf.Sign(distance);
                float vy = velocity * Mathf.Sin(angle);

                rb.velocity = new Vector2(vx, vy);
            }
        }
    }
}