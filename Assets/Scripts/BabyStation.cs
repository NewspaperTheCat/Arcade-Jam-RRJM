using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyStation : Station
{

    [Header("Explosion")]
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionRechargeTime;

    private float currentExplosionRechargeTime;

    protected override void Start()
    {
        Charge();

        base.Start();

    }

    private void Charge() { 
        Renderer rend = transform.GetChild(0).GetComponent<Renderer>();
        rend.material.color = Color.green;
        currentExplosionRechargeTime = explosionRechargeTime;
        stationState = StationState.Normal;
    }


    private void Update()
    {
        switch (stationState)
        {
            case StationState.Normal:
                break;
            case StationState.Cooldown:
                Recharge();
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (!enableGizmos) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public override void OnHit()
    {
        if (stationState == StationState.Cooldown)
            return;

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, ~LayerMask.GetMask("Ground"), QueryTriggerInteraction.Collide);
        foreach (Collider col in hits)
        {
            if (col.transform.GetComponent<Enemy>() != null)
            {
                col.transform.GetComponent<Enemy>().TakeDamage(50);
            }
        }

        stationState = StationState.Cooldown;
        Renderer rend = transform.GetChild(0).GetComponent<Renderer>();
        rend.material.color = Color.red;
    }

    public override void Recharge()
    {
        currentExplosionRechargeTime -= Time.deltaTime;

        if (currentExplosionRechargeTime < 0) {
            Charge();
        }
    }


}
