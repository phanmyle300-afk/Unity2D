using UnityEngine;

public class Guard : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField] Transform wallDetection;
    [SerializeField] Transform groundCheck;
    [SerializeField] private bool movingRight = true;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    private Rigidbody2D rb;
    private Transform player;
    private bool isGrounded;
    private bool checkWall;
    public bool isPlayerDetected = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);
        checkWall = Physics2D.OverlapCircle(wallDetection.position, 0.25f, wallLayer);
        Patrol();
    }

    private void Patrol()
    {
        rb.velocity = new Vector2((movingRight ? 1 : -1) * moveSpeed, rb.velocity.y);

        if (!isGrounded || checkWall)
        {
            Flip();
        }
    }

    private void Flip()
    {
        movingRight = !movingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerDetected = true;
            player = collision.transform;
            Debug.Log("Player detected, turning to face.");
        }
    }
}
