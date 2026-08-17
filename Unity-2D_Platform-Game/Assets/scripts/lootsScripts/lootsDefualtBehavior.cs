using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lootsDefualtBehavior : MonoBehaviour
{
    private string healthPosionName;
    private float health = 0;

    [Header("health posion settings")]
    public float healthGrowthRate = 2;
    public float lifeTime = 5;
    public float magnitudeX_forBounce = 10f;

    private float lifeTimeCounter = 0;

    private Rigidbody2D Loot_rigidbody2;

    private void Start()
    {
        healthPosionName = this.gameObject.name;

        switch (healthPosionName)
        {
            case "PosionLVL1(Clone)":

                health = 10;

                break;

            case "PosionLVL2(Clone)":

                health = 10 * healthGrowthRate;

                break;

            case "PosionLVL3(Clone)":

                health = 10 * (healthGrowthRate + 1);

                break;

            case "PosionLVL4(Clone)":

                health = 10 * (healthGrowthRate + 2);

                break;

            default:

                health = 10 / 2;

                break;
        }

        Loot_rigidbody2 = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        deathControl();
    }
    public float getHealth() { return health; }

    private void deathControl()
    {
        lifeTimeCounter += Time.deltaTime;

        if (lifeTime < lifeTimeCounter)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            Loot_rigidbody2.velocity = Vector3.zero;
            Loot_rigidbody2.AddForce(Vector2.up * magnitudeX_forBounce, ForceMode2D.Impulse);
        }
    }
}
