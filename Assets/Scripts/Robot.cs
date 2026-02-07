using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Robot : MonoBehaviour
{
    [SerializeField] float speed;

    Vector2 velocity;

    [Header("Charge")]
    float charge = 1; // max charge = 1, empty = 0
    [SerializeField] Image ChargeBar;
    [SerializeField] float passiveDecay;

    // Update is called once per frame
    void Update() {
        velocity = InputManager.inst.GetRobotMovement() * speed;

        // Apply velocity
        transform.position += new Vector3(velocity.x, 0, velocity.y) * Time.deltaTime;

        // Clamp position
        if (transform.position.magnitude > 10) {
            transform.position = transform.position.normalized * 10;
        }

        // Passive decay
        charge -= passiveDecay * Time.deltaTime;
        UpdateChargeMeter();
    }

    // leave amount -1 for full
    public void Recharge(float amount = -1) {
        if (amount == -1)
            charge = 1;
        else
            charge += amount;
        UpdateChargeMeter();
    }

    private void UpdateChargeMeter() {
        ChargeBar.fillAmount = charge;
    }
}
