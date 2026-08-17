using System.Collections;
using UnityEngine;

public class Dog : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb2d;

    [SerializeField] private Transform wallDetection;
    [SerializeField] private Transform edgeCheck;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform player;
    public LayerMask groundLayer;
    bool edgeHit;
    public float moveSpeed;
    private bool isGrounded;
    private bool isRunning = false;
    private bool isPlayerDetected = false;
    private bool isAttacking = false;
    private float counter = 0;
    public float waitTime = 2f;
    public float damage = 5f;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckWallOrEdge();
        CheckGrounded();
        CheckPlayerDetection();

        if (!isPlayerDetected)
            Patrol();
        else
        ChasePlayer();

        Attack();
    }

    private void Patrol()
    {
        rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
    }

    private void ChasePlayer()
    {
        animator.SetBool("IsAttacking", isAttacking);

        if (transform.position.x + 1 - player.position.x < 1f || transform.position.x - 1 - player.position.x < 1f){
            Debug.Log("bekle");
        }
        else if (player.position.x > transform.position.x && moveSpeed < 0)
                    Flip();
                else if (player.position.x < transform.position.x && moveSpeed > 0)
                    Flip();

        rb2d.velocity = new Vector2(Mathf.Sign(moveSpeed) * Mathf.Abs(moveSpeed), rb2d.velocity.y);
    }

    private void CheckWallOrEdge()
    {
        var wallHit = Physics2D.Raycast(wallDetection.position, Vector2.right * Mathf.Sign(moveSpeed), 0.25f);
        edgeHit = Physics2D.OverlapCircle(edgeCheck.position, 0.25f, groundLayer);
        
        if (wallHit.collider != null && wallHit.collider.CompareTag("Wall"))
        {
            Flip();
        }

        if (isGrounded && !edgeHit)
        {
            Debug.Log("FLIP: Edge Detected!");
            Flip();
        }
    }

    private void Attack()
    {
        if (isAttacking)
        {
            counter += Time.deltaTime;
            if (counter > waitTime)
            {
                player.GetComponent<healthControl>().takeDamege(damage);
                counter = 0;
            }
        }
    }

    private void CheckPlayerDetection()
    {
        var hit = Physics2D.Raycast(wallDetection.position, Vector2.right * Mathf.Sign(moveSpeed), 8f);
        Debug.DrawRay(wallDetection.position, Vector2.right * Mathf.Sign(moveSpeed) * 8f, Color.blue);

        if (hit && hit.collider.CompareTag("Player") && !isRunning)
        {
            player = hit.collider.transform;
            isRunning = true;
            isPlayerDetected = true;
            StartCoroutine(Run());
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);
    }

    private IEnumerator Run()
    {
        moveSpeed *= 3;
        yield return new WaitForSeconds(3f);
        moveSpeed /= 3;
        isRunning = false;
    }

    private void Flip()
    {
        moveSpeed *= -1;
        transform.eulerAngles = new Vector3(0, moveSpeed > 0 ? 0 : 180, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered detection area.");
            isAttacking = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered detection area.");
            isAttacking = false;
            counter = 0;
        }
    }
}
