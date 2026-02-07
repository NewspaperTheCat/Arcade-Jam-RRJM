using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    None,
    Grub,
}
public enum EnemyState
{
    Moving,
    InRange,
    Attack
}

public abstract class Enemy : MonoBehaviour {

    [Header("Type")]
    [SerializeField] protected int maxHealth;
    [SerializeField] EnemyType enemyType = EnemyType.None;

    [Header("Spawn Value")]
    [SerializeField] protected int cost;

    [Header("Distance")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float enemyArriveDistance;

    [Header("Attack")]
    [SerializeField] protected int attackDamage;
    [SerializeField] protected float arrivalAttackCooldown;
    [SerializeField] protected float loopAttackCooldown;

    [Space(10)]
    [Header("Debug")]
    [SerializeField] protected bool enableGizmos;

    //Currrent Values
    protected int currentHealth;
    protected float currentAttackCooldown;

    //States
    protected bool justArrived;
    protected EnemyState enemyState = EnemyState.Moving;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        justArrived = false;
        currentAttackCooldown = arrivalAttackCooldown;

        if (enemyType == EnemyType.None)
        {
            Debug.LogError("You didn't set the Type Dummy!");
        }
    }
    private void OnDrawGizmos()
    {
        if (!enableGizmos) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyArriveDistance);
    }

    public abstract void Attack();
    public abstract void Move(Transform target);

    public abstract void InRange(Transform target);

    public virtual void DealDamage(Station station)
    {
        station.TakeDamage(attackDamage);
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
