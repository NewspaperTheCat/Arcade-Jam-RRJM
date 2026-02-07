using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grub : Enemy
{
    protected override void Update()
    {
        Move(WorldManager.Instance.homeStation);
    }
    public override void Move(Transform target)
    {
        if(target == null)
        {
            Debug.LogError("Need a Target!");
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime);

    }
}
