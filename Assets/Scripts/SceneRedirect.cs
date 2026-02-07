using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRedirect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InputManager.inst.smite.AddListener(DoTheThing);
    }

    public void DoTheThing()
    {
        GameManager.Instance.RestartGame();
    }
}
