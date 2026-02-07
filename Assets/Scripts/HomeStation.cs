using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeStation : Station
{
    public override void Die()
    {
        base.Die();
        GameManager.Instance.SetGameOver();
    }
}
