using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.GraphicsBuffer;

public class Grub : Enemy
{
    // Animation
    Animator animator;

    protected override void Start()
    {
        base.Start();

        animator = GetComponentInChildren<Animator>();
    }

    protected void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Moving:
                Move(currentTarget);
                break;
            case EnemyState.InRange:
                InRange(currentTarget);
                break;
            case EnemyState.Attack:
                break;
        }
    }
    public override void Move(Transform target)
    {
        if(target == null)
        {
            GetNewTarget();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * moveSpeed);
        transform.LookAt(target.position);

        if (Vector3.Distance(transform.position, target.position) <= enemyArriveDistance) {
            enemyState = EnemyState.InRange;
        }

    }

    public override void InRange(Transform target)
    {
        if (target == null || !target.GetComponent<Station>().IsAlive())
        {
            GetNewTarget();
            return;
        }

        currentAttackCooldown -= Time.deltaTime;

        if (currentAttackCooldown <= 0)
        {
            currentAttackCooldown = loopAttackCooldown;
            enemyState = EnemyState.Attack;
            Attack();
        }
    }
    public override void Attack()
    {
        animator.SetTrigger("Attack");
        Invoke("DealDamage", dealDamageDelay);
        justArrived = false;
        enemyState = EnemyState.InRange;
    }

    
}

