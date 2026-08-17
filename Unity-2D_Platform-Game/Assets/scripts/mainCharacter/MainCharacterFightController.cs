using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [Header("damage settings")]
    public float damage = 5;
    public float damageWaitingTime = 0.2f;
    private float damageTimeCounter = 0;
    public GameObject Bullet;
    public float bulletVectorMagnitude = 1;
    private float bulletTimeCounter = 0;
    public float bulletTime = 0.2f;
    public float offset = 0;
    public bool hasVacaine = false;
    public LayerMask Enemys;
    public float punchRadius;

    private Animator PlayerAnimator;
 

    private void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        PunchAttackControl();
        Shotattack();
    }

    private void PunchAttackControl()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           
            PlayerAnimator.SetTrigger("isAttacking");
            punch();
          
        }
    }

    private void punch()
    {
        Collider2D[] enemys = Physics2D.OverlapCircleAll(this.gameObject.transform.position + new Vector3(0, offset, 0), punchRadius, Enemys);

        foreach (Collider2D enemy in enemys)
        {
            if (enemy != null)
            {
                if (enemy.gameObject.GetComponent<healthControl>() != null)
                {
                    enemy.gameObject.GetComponent<healthControl>().takeDamege(damage);
                }
            }
        }

    }

    private void Shotattack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            PlayerAnimator.SetTrigger("isShot");

            GameObject Clone = Instantiate(Bullet, this.gameObject.transform.position + new Vector3(0, offset, 0), this.gameObject.transform.rotation);
            Clone.SetActive(true);
            Clone.GetComponent<Rigidbody2D>().gravityScale = 0;

            if (this.gameObject.transform.rotation.y != 0)
            {
                Clone.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletVectorMagnitude, ForceMode2D.Impulse);
            }
            else
            {
                Clone.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletVectorMagnitude, ForceMode2D.Impulse);
            }
          
        }
    }
   
    private void shotTimer()
    {
        bulletTimeCounter += Time.deltaTime;

        if (bulletTime < bulletTimeCounter)
        {
            Shotattack();
            bulletTimeCounter = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.gameObject.transform.position + new Vector3(0, offset, 0), punchRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag.Equals("Asi"))
        {
            hasVacaine = true;
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("ball"))
        {
            if (Bullet.name.Split("(")[0] != collision.gameObject.name.Split("(")[0])
            {
                Destroy(Bullet);
                Bullet = collision.gameObject;
                collision.gameObject.SetActive(false);
            }
        }

        if (collision.gameObject.tag.Equals("HealthPosion"))
        {
            this.gameObject.GetComponent<healthControl>().takeHealth(collision.gameObject.GetComponent<lootsDefualtBehavior>().getHealth());
            Destroy(collision.gameObject);
        }
    }

}
