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

    Item carrying = null;

    void Start() {
        InputManager.inst.interact.AddListener(Interact);
    }

    // Update is called once per frame
    void Update() {
        velocity = InputManager.inst.GetRobotMovement() * speed;

        // Apply velocity
        transform.position += new Vector3(velocity.x, 0, velocity.y) * Time.deltaTime;

        // Clamp position
        if (transform.position.magnitude > 10) {
            transform.position = transform.position.normalized * 10;
        }

        // set rotation
        if (velocity.magnitude > 0) {
            float target = Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg;
            // transform.eulerAngles += Vector3.up * (target - transform.eulerAngles.y) * Time.deltaTime / .5f;
            transform.eulerAngles = Vector3.up * target;
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

    public void PickUpItem(Item item) {
        carrying = item;
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.up * 1.125f;
        item.SetAnchor();
    }

    public void Interact() {
        Debug.Log("Carrying item? " + carrying);
    }
}
