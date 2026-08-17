using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class GuardWithProtection : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField] Transform wallDetection;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform protection;
    [SerializeField] Transform edgeCheck;
    private bool movingRight = true;
    private Rigidbody2D rb2d;
    public bool isGrounded;
    public bool isDashing;
    public bool isPlayerDetected = false;
    [SerializeField]
    private Transform player;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    LayerMask wallLayer;
    public bool canDash = true;
    bool edgeHit;
    public float damage = 10;
    

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isPlayerDetected)
        {
            CheckGrounded();
            CheckWallCollision();

            rb2d.velocity = new Vector2((movingRight ? 1 : -1) * moveSpeed, rb2d.velocity.y);
        }
        else
        {
            if (player != null && canDash)
            {
                if (player.position.x > transform.position.x && !movingRight)
                    Flip();
                else if (player.position.x < transform.position.x && movingRight)
                    Flip();
                rb2d.velocity = new Vector2((movingRight ? 1 : -1) * moveSpeed, rb2d.velocity.y);
            }
        }
        CheckPlayerAndCanDash();
    }

    private void CheckPlayerAndCanDash()
    {
        RaycastHit2D horizontalPlayerInfo = Physics2D.Raycast(protection.position, (movingRight ? 1 : -1) * Vector2.right, 8f);
        if (horizontalPlayerInfo)
        {
            if (horizontalPlayerInfo.collider.CompareTag("Player") && canDash)
            {
                canDash = false;
                StartCoroutine(Dash()); 
            }
        }
    }

    private void CheckWallCollision()
    {
        if (!isDashing)
        {
            edgeHit = Physics2D.OverlapCircle(edgeCheck.position, 0.25f, groundLayer);

            bool wallInfo = Physics2D.OverlapCircle(wallDetection.position, .25f, wallLayer);
            
            if (!edgeHit) Flip();
        }


    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);
    }

    IEnumerator Dash()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(2f);
        isDashing = true;
        moveSpeed = 30f;
        yield return new WaitForSeconds(.25f);
        moveSpeed = 0f;
        rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
        yield return new WaitForSeconds(2f);
        moveSpeed = 5f;
        isDashing = false;
        yield return new WaitForSeconds(2f);
        canDash = true;
    }
    private void Flip()
    {
        movingRight = !movingRight;
        transform.eulerAngles = new Vector3(0, movingRight ? 0 : -180, 0);


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerDetected = true;
            player = collision.transform;
            collision.gameObject.GetComponent<healthControl>().takeDamege(damage);
        }
    }
}