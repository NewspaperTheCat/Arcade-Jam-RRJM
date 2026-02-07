using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StationType
{
    None,
    HomeStation,
    BabyStation
}

public abstract class Station: MonoBehaviour{

    [Header("Debug")]
    [SerializeField] protected bool enableGizmos;
    [Space(10)]

    [Header("Type")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected StationType stationType;

    protected int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        if (stationType == StationType.None)
        {
            Debug.LogError("You Didn't Set the Type Dummy!");
        }
    }
    
    public StationType getStationType()
    {
        return stationType;
    }

    public virtual void OnHit()
    {
        return;
    }

    public virtual void Recharge()
    {
        return;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        GameManager.Instance.AddPoints(100);
        Destroy(gameObject);
    }
}
