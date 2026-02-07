using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyStation : Station
{

    protected override void Start()
    {
        Renderer rend = transform.GetChild(0).GetComponent<Renderer>();
        rend.material.color = Color.green;
    
        base.Start();

    }

    [SerializeField] private float explosionRadius;
    private void OnDrawGizmos()
    {
        if (!enableGizmos) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public override void OnHit()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, ~LayerMask.GetMask("Ground"), QueryTriggerInteraction.Collide);
        foreach (Collider col in hits)
        {
            if (col.transform.GetComponent<Enemy>() != null)
            {
                col.transform.GetComponent<Enemy>().TakeDamage(50);
            }
        }
    }
}
