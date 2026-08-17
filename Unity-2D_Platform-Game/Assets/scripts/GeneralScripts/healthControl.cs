using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthControl : MonoBehaviour
{
    [Header("health system")]
    public float health;
    private float perCentForHealth = 1;
    public float maxhealth;
    public GameObject healthBar;
    public GameObject leftPointer;
    public GameObject healthBar_slider;


    [Header("effect system")]
    public float waiting_period = 0.2f;
    public float cameraDamagePosition = 0.1f;
    private float timeCounter = 0;
    private bool isDamage = false;
    private float healthBarLocalScale_x_right;
    private float healthBarLocalScale_x_left;
    public Vector2 damageForce;
    private float damageForcexNegative;
    private float damageForcexPositive;

    private deathScript deathScript;
    private GameObject MainCamera;
    private GameObject Player;

    private void Awake()
    {
        deathScript = FindAnyObjectByType<deathScript>();    
    }

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");

        MainCamera = GameObject.FindWithTag("MainCamera");

        damageForcexNegative = -damageForce.x;
        damageForcexPositive = damageForce.x;

        healthBar.SetActive(false);

        health = maxhealth;

        healthBarLocalScale_x_right = healthBar.transform.localScale.x;
        healthBarLocalScale_x_left = healthBar.transform.localScale.x * (-1);
    }
    private void Update()
    {
        if (isDamage && this.gameObject.tag.Equals("Player"))
        {
            damageEffect();
        }
        staticHealthBar(this.gameObject.GetComponent<Transform>());
    }

    public void takeDamege(float damage)
    {
        healthBar.SetActive(true);

        health = health - damage;

        refreshHealtBar();

        damageShift();

        isDamage = true;

        if (health <= 0)
        {
            getDeath();
        }
    }

    public void takeDamegeAnimals(float damage, bool asiVarmi)
    {
        if (asiVarmi)
        {
            healthBar.SetActive(true);

            health = health - damage;

            refreshHealtBar();

            damageShift();

            isDamage = true;

            if (health <= 0)
            {
                getDeath();
            }
        }
    }

    public void takeHealth(float Health)
    {
        if (health != maxhealth)
        {
            health = health + Health;
            if (health >= maxhealth)
            {
                health = maxhealth;
            }
            refreshHealtBar();
        }
    }

    private void refreshHealtBar()
    {
        perCentForHealth = health / maxhealth;
        healthBar_slider.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, health / maxhealth);
        leftPointer.transform.localScale = new Vector3(perCentForHealth, leftPointer.transform.localScale.y, leftPointer.transform.localScale.z);
    }

    public void getDeath()
    {
        if (this.gameObject.tag.Equals("Enemy") || this.gameObject.tag.Equals("Enemy(Animals)"))
        {
            Instantiate(MainCamera.GetComponent<enemyDeathBehavior>().DropLoot(), this.gameObject.transform.position + new Vector3(0, 0.5f, 0), this.gameObject.transform.rotation);
        }

        if(this.gameObject.tag.Equals("Player"))
        {
            deathScript.ShowDeathMenu();
        }

        Destroy(this.gameObject.gameObject);
    }

    private void staticHealthBar(Transform GameObject_Transform)
    {
        if (GameObject_Transform.rotation.eulerAngles.y == 180 || GameObject_Transform.localScale.x < 0)
        {
            healthBar.transform.localScale = new Vector3(healthBarLocalScale_x_left, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        }
        else
        {
            healthBar.transform.localScale = new Vector3(healthBarLocalScale_x_right, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        }
    }

    private void damageEffect()
    {
        float x = Random.Range(-cameraDamagePosition, cameraDamagePosition);
        float y = Random.Range(-cameraDamagePosition, cameraDamagePosition);
        MainCamera.transform.position = MainCamera.transform.position + new Vector3(x, y, 0);
        timeCounter += Time.deltaTime;

        if (timeCounter > waiting_period)
        {
            isDamage = false;
            timeCounter = 0;
        }
    }

    private void damageShift()
    {

        if (Player.transform.rotation.y == 180)
        {
            damageForce.x = damageForcexPositive;
        } else
        {
            damageForce.x = damageForcexNegative;
        }

        if (this.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(damageForce, ForceMode2D.Impulse);
        }
    }

}
