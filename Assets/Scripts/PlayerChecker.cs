using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerChecker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI p1Connectivity, p2Connectivity;
    bool p1Joined, p2Joined;
    const String waitingText = "-> JOIN <-", readyText = "Ready!";
    bool starting = false;


    // Start is called before the first frame update
    void Start()
    {
        p1Joined = false;
        p2Joined = false;

        p1Connectivity.text = waitingText;
        p2Connectivity.text = waitingText;

        InputManager.inst.buildStart.AddListener(P1Connected);
        InputManager.inst.smite.AddListener(P2Connected);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.inst.GetRobotMovement() != Vector2.zero) {
            P1Connected();
        }
        if (InputManager.inst.GetZeusMovement() != Vector2.zero) {
            P2Connected();
        }

        if (!starting && p1Joined && p2Joined) {
            starting = true;
            p1Connectivity.text = "Starting...";
            p2Connectivity.text = "Starting...";
            Invoke("ToGame", 1);
        }
    }

    void P1Connected() {
        if (starting || p1Joined) return;
        p1Connectivity.text = readyText;
        p1Joined = true;
    }

    void P2Connected() {
        if (starting || p2Joined) return;
        p2Connectivity.text = readyText;
        p2Joined = true;
    }

    private void ToGame() {
        SceneManager.LoadScene("Game");
    }
}
