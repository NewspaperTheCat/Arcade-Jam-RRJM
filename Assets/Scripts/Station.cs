using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StationType
{
    None,
    HomeStation
}

public abstract class Station: MonoBehaviour{

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
        Destroy(gameObject);
    }
}
