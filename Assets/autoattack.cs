using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoattack : MonoBehaviour
{

    private GameObject enemy;
    public Animator anim;
    public int atkRange;

    void Start()
    {
        enemy = GameObject.FindWithTag("Enemy");
        GameObject child = transform.GetChild(0).gameObject;
        anim = child.GetComponent<Animator>();
    }

    void Update()
    {
        //Makes the towers look towards the enemy
        transform.LookAt(enemy.transform.position);
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        if (distance <= atkRange)
        {
            anim.Play("Attack1");
        }
        
    }
}
