using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Station: MonoBehaviour{

    [SerializeField] private int maxHealth;

    private int currentHealth;

    [HideInInspector] public enum StationType
    {
        None,
        HomeStation
    }

    [SerializeField] private StationType stationType;

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
