using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonousPlant : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer2D;
    float poisonTimer = 0f;
    float poisonInterval = .5f;
    public float scaleLimitX = 8f;
    public float scaleLimitY = 6f;


    void Update()
    {
        poisonTimer += Time.deltaTime;

        if (poisonTimer > poisonInterval)
        {
            Vector3 curScale = spriteRenderer2D.transform.localScale;
            if (curScale.x < scaleLimitX && curScale.y < scaleLimitY)
            {
                curScale = new Vector3(curScale.x + .5f, curScale.y + .5f, 0f);
                spriteRenderer2D.transform.localScale = curScale;
            }
            else
            {
                poisonInterval = .1f;
                Color tmp = spriteRenderer2D.color;
                tmp.a -= .05f;
                spriteRenderer2D.color = tmp;
                if (tmp.a <= 0f)
                {
                    spriteRenderer2D.enabled = false;
                }
            }
            poisonTimer = 0f;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player poisoned");
            //DO some damage mann
        }
    }
}