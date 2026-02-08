using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StationType
{
    None,
    HomeStation,
    BabyStation
}

public enum StationState
{
    Normal,
    Cooldown
}

public abstract class Station: MonoBehaviour{

    [Header("Debug")]
    [SerializeField] protected bool enableGizmos;
    [Space(10)]

    [Header("Type")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected MeshRenderer meshRenderer;
    [SerializeField] protected StationType stationType;

    protected int currentHealth;

    protected StationState stationState = StationState.Normal;

    protected virtual void Start()
    {
        // make material unique
        Material clone = Instantiate<Material>(meshRenderer.materials[0]);
        meshRenderer.materials[0] = clone;

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
        Debug.Log("took damage: " + amount);
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
        else {
            float pHealth = currentHealth / (float)maxHealth;
            Debug.Log(pHealth);
            meshRenderer.materials[0].SetVector("_EmissionColor", new Vector4(pHealth, pHealth, pHealth, 1.0f));
        }
    }
    public virtual void Die()
    {
        meshRenderer.materials[0].SetVector("_EmissionColor", new Vector4(0, 0, 0, 1.0f));
        GameManager.Instance.AddPoints(100);
        Destroy(gameObject);
    }
}
