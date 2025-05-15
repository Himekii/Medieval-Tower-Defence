using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towers : MonoBehaviour
{

    private GameObject closestEnemy;
    public GameObject self;
    public Animator anim;
    public int atkRange;
    public bool isRanged;
    private Enemy enemyClass;
    public int damage = 0;

    void Start()
    {
        GameObject child = transform.GetChild(0).gameObject;
        anim = child.GetComponent<Animator>();
    }

    Enemy GetClosestEnemy(GameObject[] enemies)
    {
        float minDist = Mathf.Infinity;
        foreach (GameObject e in enemies)
        {
            float dist = Vector3.Distance(e.transform.position, transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestEnemy = e;
            }

        }
        return closestEnemy.GetComponent<Enemy>();
    }



    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            enemyClass = GetClosestEnemy(enemies);
            //Makes the towers look towards the enemy
            transform.LookAt(closestEnemy.transform.position);
            float distance = Vector3.Distance(transform.position, closestEnemy.transform.position);
            if (distance <= atkRange)
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                {
                    anim.Play("Attack1");
                    enemyClass.health -= damage;
                }
            }
        }
    }
};
