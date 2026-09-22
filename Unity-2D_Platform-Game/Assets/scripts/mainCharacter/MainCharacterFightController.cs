using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Chặn đấm khi chạm UI

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
        if (IsPointerOverUI()) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerAnimator.SetTrigger("isAttacking");
            punch();
        }
    }

    bool IsPointerOverUI()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                {
                    return true;
                }
            }
        }
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
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

            if (Bullet != null)
            {
                GameObject Clone = Instantiate(Bullet, this.gameObject.transform.position + new Vector3(0, offset, 0), this.gameObject.transform.rotation);
                Clone.SetActive(true);
                Rigidbody2D rb = Clone.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale = 0;
                    if (this.gameObject.transform.rotation.y != 0)
                    {
                        rb.AddForce(Vector2.left * bulletVectorMagnitude, ForceMode2D.Impulse);
                    }
                    else
                    {
                        rb.AddForce(Vector2.right * bulletVectorMagnitude, ForceMode2D.Impulse);
                    }
                }
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
            if (Bullet != null && Bullet.name.Split("(")[0] != collision.gameObject.name.Split("(")[0])
            {
                Destroy(Bullet);
                Bullet = collision.gameObject;
                collision.gameObject.SetActive(false);
            }
        }

        if (collision.gameObject.tag.Equals("HealthPosion"))
        {
            healthControl hc = this.gameObject.GetComponent<healthControl>();
            lootsDefualtBehavior loot = collision.gameObject.GetComponent<lootsDefualtBehavior>();
            if (hc != null && loot != null)
            {
                hc.takeHealth(loot.getHealth());
            }
            Destroy(collision.gameObject);
        }
    }
}