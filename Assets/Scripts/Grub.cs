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
                Move(WorldManager.Instance.homeStation);
                break;
            case EnemyState.InRange:
                InRange(WorldManager.Instance.homeStation);
                break;
            case EnemyState.Attack:
                break;
        }
    }
    public override void Move(Transform target)
    {
        if(target == null)
        {
            Debug.LogError("Need a Target!");
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * moveSpeed);

        if (Vector3.Distance(transform.position, target.position) <= enemyArriveDistance) {
            enemyState = EnemyState.InRange;
        }

    }

    public override void InRange(Transform target)
    {
        if (target == null)
        {
            Debug.LogError("Need a Target!");
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
        DealDamage(WorldManager.Instance.homeStation.GetComponent<Station>());
        justArrived = false;
        enemyState = EnemyState.InRange;
    }
}

