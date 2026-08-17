using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperPlant : MonoBehaviour
{
    Rigidbody2D rb2d;
    float moveSpeed = 3f;
    bool canJump = false;
    bool movingRight = true;
    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    Transform wallCheck;
    [SerializeField]
    LayerMask groundLayer;
    bool isGrounded;
    public float damage = 20;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            Debug.Log("Null rigidbody");
        }
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.05f, groundLayer);

        if (isGrounded)
        {
            canJump = true;
        }

        RaycastHit2D wallInfo = Physics2D.Raycast(wallCheck.position, Vector2.right, .2f * (movingRight ? 1 : -1));
        if (wallInfo && wallInfo.collider.CompareTag("Wall"))
        {
            movingRight = !movingRight;
            transform.eulerAngles = new Vector3(0, movingRight ? 0 : -180, 0);
        }

    }
    void FixedUpdate()
    {
        if (canJump)
        {   
            StartCoroutine(Jump());
        }
        rb2d.velocity = new Vector2((movingRight ? 1 : -1) * moveSpeed, rb2d.velocity.y);

    }
    IEnumerator Jump()
    {
        canJump = false;
        Vector2 randJumpForce = new Vector2(0f, Random.Range(3, 6));
        rb2d.AddForce(randJumpForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(2f);
        rb2d.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<healthControl>().takeDamege(damage);
        }
    }
}