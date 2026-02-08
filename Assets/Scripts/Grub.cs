using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.GraphicsBuffer;

public class Grub : Enemy
{
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
        DealDamage(currentTarget.GetComponent<Station>());
        justArrived = false;
        enemyState = EnemyState.InRange;
    }
}

