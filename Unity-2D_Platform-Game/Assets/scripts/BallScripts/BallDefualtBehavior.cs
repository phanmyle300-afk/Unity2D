using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDefualtBehavior : MonoBehaviour
{
    private Rigidbody2D BallRigidBody2D;

    [Header("ball defualt settings")]
    public float magnitudeX_forBounce = 2f;
    public float defualtGravityScale = 2f;
    public float damage = 1f;
    private float lifeTime = 3f;
    private void Start()
    {
        BallRigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isDeath();
    }

    private void isDeath()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (BallRigidBody2D.velocity.x == 0)
        {
            if (collision.gameObject.tag.Equals("Ground"))
            {
                BallRigidBody2D.velocity = Vector3.zero;
                BallRigidBody2D.AddForce(Vector2.up*magnitudeX_forBounce,ForceMode2D.Impulse);
            }
        } 
        else
        {
            if (collision.gameObject.tag.Equals("Ground") || collision.gameObject.tag.Equals("Wall") || collision.gameObject.tag.Equals("Ceilling"))
            {
                Destroy(this.gameObject);
            }

            if (collision.gameObject.tag.Equals("Enemy"))
            {
                healthControl health = collision.gameObject.GetComponent<healthControl>();
                if (health != null)
                {
                    health.takeDamege(damage);
                }
                Destroy(this.gameObject);
            }

            if (collision.gameObject.tag.Equals("Enemy(Animals)")) 
            {
                healthControl health = collision.gameObject.GetComponent<healthControl>();
                if (health != null)
                {
                    health.takeDamegeAnimals(damage, GameObject.FindWithTag("Player").GetComponent<MainCharacter>().hasVacaine);
                }
                Destroy(this.gameObject);
            }

            if (collision.gameObject.tag.Equals("Protection"))
            {
                Destroy(this.gameObject);
            }

        }
    }
}
