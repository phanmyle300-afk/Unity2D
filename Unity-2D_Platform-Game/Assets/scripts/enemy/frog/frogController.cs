using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogController : MonoBehaviour
{

    [Header("frog movement settings")]
    public float x_defualtMovement = 1;
    public float y_defualtMovement = 1;
    public float defualt_MovmmentMagnitude = 1;
    public float maxJumpNumber = 10;

    private bool isOnGround = false;

    [Header("frog attack settings")]
    public GameObject bullet;
    public float punchDamage = 5;
    public float shotRange = 10;
    public float shotVectorMagnitude = 1;
    public float WaitingTime = 2f;
    private float counter = 0;
    private bool isStop = false;

    private float distance;
    private bool onCollisionPlayer = false;
    private bool isRight = true;
   
    private Vector2 shot;
    private Vector2 force;
    private float JumpCounter = 0;

    private GameObject player;
    private healthControl PlayerhealthControl;
    private Animator frogAnimator;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        PlayerhealthControl = player.GetComponent<healthControl>();
        frogAnimator = this.gameObject.GetComponent<Animator>();
        distance = Vector2.Distance(player.transform.position,this.gameObject.transform.position);
    }

    private void Update()
    {
        distanceCounter();
        stop();
        WaitingCounter();
    }

    private void distanceCounter()
    {
        distance = Vector2.Distance(player.transform.position, this.gameObject.transform.position);
    }

    private void getForce()
    {
        if (isOnGround)
        {

            frogAnimator.SetTrigger("isJumping");  
            if (isRight)
            {
                force = new Vector2(x_defualtMovement * defualt_MovmmentMagnitude, y_defualtMovement * defualt_MovmmentMagnitude);
            }
            else
            {
                force = new Vector2(-x_defualtMovement * defualt_MovmmentMagnitude, y_defualtMovement * defualt_MovmmentMagnitude);
            }

            JumpCounter++; 
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            isOnGround = false;
        }
    }

    private void stop()
    {
        if (JumpCounter > maxJumpNumber)
        {
            frogAnimator.SetBool("isOnDamage", true);
            JumpCounter = 0;
            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            isStop = true;
        }
    }

    private void WaitingCounter()
    {
        if (isStop)
        {
            counter += Time.deltaTime;

            if (WaitingTime < counter)
            {
                frogAnimator.SetBool("isOnDamage", false);
                isStop = false;
                isOnGround = true;
                counter = 0;
                getForce();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            isOnGround = true;

            if (distance < shotRange && !onCollisionPlayer)
            {
                frogAnimator.SetTrigger("isShoting");
                GameObject clone = Instantiate(bullet, this.gameObject.transform.position, this.gameObject.transform.rotation);
              
                if (isRight)
                {
                    shot = Vector2.right * shotVectorMagnitude;
                }
                else
                {
                    shot = Vector2.left * shotVectorMagnitude;
                }

                clone.GetComponent<Rigidbody2D>().AddForce(shot, ForceMode2D.Impulse);
            }
            getForce();
        }

        if (collision.gameObject.tag.Equals("Wall"))
        {
            if (this.gameObject.transform.localScale.x > 0)
            {
                this.gameObject.transform.localScale = new Vector2(-this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y);
                isRight = false;
            } 
            else
            {
                this.gameObject.transform.localScale = new Vector2(-this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y);
                isRight = true;
            }
        }

        if (collision.gameObject.tag.Equals("Player"))
        {
            frogAnimator.SetTrigger("isPunchAttacking");
            PlayerhealthControl.takeDamege(punchDamage);
        }
    }
}
