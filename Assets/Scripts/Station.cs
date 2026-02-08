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
    [SerializeField] protected ParticleSystem destructionParticles;

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

        // Random rotation
        transform.Rotate(Vector3.up * Random.Range(0, 360));
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
        StartCoroutine("DeathSequence");
    }

    IEnumerator DeathSequence() {
        float total =  destructionParticles.main.duration + 2;
        float duration = total;

        destructionParticles.Play();

        float height = 1.5f;
        Vector3 xzStart = new Vector3(transform.position.x, 0, transform.position.z);

        while (duration > 0) {
            duration -= Time.deltaTime;

            Vector3 shake = new Vector3(Random.Range(-.125f, .125f), 0, Random.Range(-.125f, .125f));
            transform.position = xzStart + shake - Vector3.up * (height * (1 - duration / total));

            destructionParticles.transform.position -= Vector3.up * destructionParticles.transform.position.y;

            yield return new WaitForEndOfFrame();
        }
        
        // At the end destroy this
        Destroy(gameObject);
    }

    public bool IsAlive() {
        return currentHealth > 0;
    }
}
