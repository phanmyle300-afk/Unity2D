using UnityEngine;

public class Worker : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField] Transform wallDetection;
    [SerializeField] Transform groundCheck;

    [SerializeField] private bool movingRight = true;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    private Rigidbody2D rb;
    private bool wallDetect;
    private bool edgeDetect;
    public bool isPlayerDetected = false;
    private Animator Animator;
    private bool isDamage = false;
    public float damage = 5;
    public float waitTimeForDamage = 0.5f;
    public float counterForTime = 0;
    private GameObject Player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        edgeDetect = Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);
        wallDetect = Physics2D.OverlapCircle(wallDetection.position, 0.25f, wallLayer);
        Patrol();
        Attack(); ;
    }

    private void Patrol()
    {
        rb.velocity = new Vector2((movingRight ? 1 : -1) * moveSpeed, rb.velocity.y);
        Animator.SetFloat("isRun",Mathf.Abs(rb.velocity.x));
       

            if (!edgeDetect || wallDetect)
            {
                Flip();
            }
       
      
    }

    private void Flip()
    {
        movingRight = !movingRight;
        this.gameObject.transform.localScale = new Vector2(-1*this.gameObject.transform.localScale.x, transform.localScale.y);
    }

    private void Attack()
    {
        if (isDamage)
        {
            counterForTime += Time.deltaTime;
            
            if (counterForTime > waitTimeForDamage)
            {
                counterForTime = 0;
                Player.GetComponent<healthControl>().takeDamege(damage);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Animator.SetBool("isAttacking", true);
            isDamage = true;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Animator.SetBool("isAttacking", false);
            isDamage = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("ball"))
        {
            Animator.SetTrigger("isHurt");
        }
    }
}
