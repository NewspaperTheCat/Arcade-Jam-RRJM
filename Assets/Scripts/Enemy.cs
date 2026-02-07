using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    [SerializeField] private int maxHealth;
    [SerializeField] private int cost;
    [SerializeField] private float moveSpeed;

    private int currentHealth;

    private enum EnemyType
    {
        None,
        Grub,
    }

    [SerializeField] private EnemyType enemyType = EnemyType.None;

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        if(enemyType == EnemyType.None)
        {
            Debug.LogError("You didn't set the Type Dummy!");
        }
    }

    //public abstract void Attack();
    public abstract void Move(Transform target);

    protected abstract void Update();
    public void TakeDamage(int amount)
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
