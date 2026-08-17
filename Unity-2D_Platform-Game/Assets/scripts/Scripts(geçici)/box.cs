using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class box : MonoBehaviour
{
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform fallCheck;
    private Rigidbody2D rb;
    public float moveSpeed;
    public bool isGrounded;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RaycastHit2D groundRay = Physics2D.Raycast(groundCheck.position, Vector2.down, 1f);
        if (groundRay.collider != null)
        {
            if (groundRay.collider.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }

        RaycastHit2D fallRay = Physics2D.Raycast(fallCheck.position, Vector2.down, 1f);
        if(!fallRay)  {
            Flip();
        }

        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

    }
    void Flip()
    {
        var tempLocal = transform.localScale;
        tempLocal.x *= -1;
        transform.localScale = tempLocal;
        moveSpeed *= -1;
    }
}
