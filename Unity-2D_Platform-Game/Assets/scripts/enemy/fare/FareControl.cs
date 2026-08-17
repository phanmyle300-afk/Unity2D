using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FareControl : MonoBehaviour
{
    public float throwAngle = 45f; // Angle for the throw
    public GameObject tukuruk; // Assign your 'tükürük' prefab in the inspector
    public Transform throwPoint; // Where the tükürük will be thrown from
    public float followSpeed = 2f; // Fare'nin takip etme hızı
    public float Range = 7f;

    private bool playerInRange = false;
    private Transform playerTransform;
    private bool isActionActive = false;
    private Animator MouseAnimator;

    public float damage = 10;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        MouseAnimator = this.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        distance();

        if (playerInRange && playerTransform != null && !isActionActive)
        {
            StartCoroutine(ActionLoop());
        }
    }

    private void distance()
    {
        float distance = Vector2.Distance(this.gameObject.transform.position, player.transform.position);
        if (distance < Range)
        {
            playerInRange = true;
            playerTransform = player.transform;
        }
        else
        {
            playerInRange = false;
            playerTransform = null;
            StopAllCoroutines();
            isActionActive = false;
        }
    }

    private IEnumerator ActionLoop()
    {
        isActionActive = true;

        while (playerInRange && playerTransform != null)
        {
            MouseAnimator.SetBool("isShooting", true);
            // 3 kere ateş et
            for (int i = 0; i < 3; i++)
            {
                ThrowTukuruk(playerTransform);
                yield return new WaitForSeconds(0.6f);
            }
            MouseAnimator.SetBool("isShooting", false);
            // 4 saniye boyunca takip et
            float followTime = 4f;
            float timer = 0f;

            while (timer < followTime && playerInRange && playerTransform != null)
            {
                // koşma anim bool true
                FollowPlayer();
                timer += Time.deltaTime;
                yield return null;
            }
        }
        // koşma anim bool false
        isActionActive = false;
    }

    private void FollowPlayer()
    {

        if (playerTransform == null) return;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // Yönü belirle (Scale ayarı)
        if (playerTransform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Sadece X ekseninde velocity ayarla
        float directionX = playerTransform.position.x > transform.position.x ? 1f : -1f;
        rb.velocity = new Vector2(directionX * followSpeed, rb.velocity.y);
    }

    void ThrowTukuruk(Transform target)
    {
        if (target == null || tukuruk == null || throwPoint == null)
            return; // Hedef yoksa tükürme

        // Hedefe dön! (yüzünü çevir)
        if (target.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Sonra tükürüğü fırlat
        GameObject Tukuruk = Instantiate(tukuruk, throwPoint.position, Quaternion.identity);
        Rigidbody2D rb = Tukuruk.GetComponent<Rigidbody2D>();
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

            // Tükürük anim     
            rb.velocity = new Vector2(vx, vy);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            player.GetComponent<healthControl>().takeDamege(damage);
        }
    }
  private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
