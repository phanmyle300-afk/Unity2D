using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDeathBehavior : MonoBehaviour
{
    [Header("Possible Loots")]

    [Header("bullet")]
    public GameObject RedBullet;
    public GameObject BlueBullet;
    public GameObject GreenBullet;
    public GameObject BlackBullet;
    public GameObject OrangeBullet;
    public GameObject PurpleBullet;

    [Header("posion")]
    public GameObject HealthPosion_LVL1;
    public GameObject HealthPosion_LVL2;
    public GameObject HealthPosion_LVL3;
    public GameObject HealthPosion_LVL4;

    [Header("foods")]
    public GameObject apple;
    public GameObject carrot;
    public GameObject banana;
    public GameObject aubergine;
    public GameObject grape;
    public GameObject garlic;
    public GameObject lemon;
    public GameObject strawBerry;
    public GameObject piper;
    public GameObject pineapple;
    public GameObject tomato;
    public GameObject corn;

    private GameObject[][] loots;
    private GameObject[] Bullets;
    private GameObject[] Posions;
    private GameObject[] foods;
    // Start is called before the first frame update
    void Start()
    {
        Bullets = new GameObject[6];
        Bullets[0] = RedBullet;
        Bullets[1] = BlueBullet;
        Bullets[2] = GreenBullet;
        Bullets[3] = BlackBullet;
        Bullets[4] = OrangeBullet;
        Bullets[5] = PurpleBullet;

        Posions = new GameObject[4];
        Posions[0] = HealthPosion_LVL1;
        Posions[1] = HealthPosion_LVL2;
        Posions[2] = HealthPosion_LVL3;
        Posions[3] = HealthPosion_LVL4;

        foods = new GameObject[12];
        foods[0] = apple;
        foods[1] = carrot;
        foods[2] = banana;
        foods[3] = aubergine;
        foods[4] = grape;
        foods[5] = garlic;
        foods[6] = lemon;
        foods[7] = strawBerry;
        foods[8] = piper;
        foods[9] = pineapple;
        foods[10] = tomato;
        foods[11] = corn;

        loots = new GameObject[3][];
        loots[0] = Bullets;
        loots[1] = Posions;
        loots[2] = foods;
    }

    public GameObject DropLoot()
    {
        int LootChateagory = Random.Range(0, loots.GetLength(0));
        int Loot = Random.Range(0, loots[LootChateagory].Length);

        return loots[LootChateagory][Loot];
    }
}
