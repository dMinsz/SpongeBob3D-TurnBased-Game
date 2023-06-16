using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack(int damage, Unit target)
    {
        
        target.TakeDamage(damage);
    }

    public override void DoSkill(int damage, Unit target)
    {
        
        target.TakeDamage(damage);
    }

    public override void TakeDamage(int damage)
    {
        animator.SetTrigger("Hit");
        base.HP -= damage;

        if (base.HP <= 0)
        {
            base.HP = 0;
            base.isDied = true;
        }
    }
}
