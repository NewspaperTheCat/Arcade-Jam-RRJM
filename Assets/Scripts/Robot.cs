using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Robot : MonoBehaviour
{
    [SerializeField] float speed;

    Vector2 velocity;

    [Header("Charge")]
    float charge = 1; // max charge = 1, empty = 0
    [SerializeField] float noChargeAmount; // max charge = 1, empty = 0
    [SerializeField] Image ChargeBar;
    [SerializeField] float passiveDecay;

    // Item related values
    [Header("Items/Stations")]
    Item carrying = null;
    [SerializeField] float buildTime;
    float buildStartTime = 0;

    public enum RobotState
    {
        Normal,
        NoPower,
        Building
    }

    private RobotState robotState;

    void Start() {
        InputManager.inst.drop.AddListener(DropItem);
        InputManager.inst.buildStart.AddListener(BuildStart);
        InputManager.inst.buildEnd.AddListener(BuildEnd);
    }

    // Update is called once per frame
    void Update() {
        switch (robotState)
        {
            case RobotState.Normal:
                NormalUpdate();
                break;
            case RobotState.NoPower:
                if (charge > noChargeAmount) {
                    robotState = RobotState.Normal;
                }
                break;
            case RobotState.Building:
                Debug.Log("progressing building");
                if (Time.time - buildStartTime >= buildTime) {
                    BuildCarrying();
                } 
                break;
        }

        // Passive decay
        charge -= passiveDecay * Time.deltaTime;
        UpdateChargeMeter();
    }

    private void NormalUpdate()
    {
        velocity = InputManager.inst.GetRobotMovement() * speed;

        // Apply velocity
        transform.position += new Vector3(velocity.x, 0, velocity.y) * Time.deltaTime;

        // Clamp position
        if (transform.position.magnitude > 10)
        {
            transform.position = transform.position.normalized * 10;
        }

        // set rotation
        if (velocity.magnitude > 0)
        {
            float target = Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg;
            // transform.eulerAngles += Vector3.up * (target - transform.eulerAngles.y) * Time.deltaTime / .5f;
            transform.eulerAngles = Vector3.up * target;
        }

        if(charge <= noChargeAmount)
        {
            robotState = RobotState.NoPower;
        }
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
        item.transform.localPosition = Vector3.forward * 1.125f;
        item.SetAnchor();
    }

    public void DropItem() {
        if (carrying != null) {
            Debug.Log("Specifically: " + carrying.cargo);
            carrying.Drop();
        }
    }

    public void BuildStart() {
        if (carrying != null) {
            robotState = RobotState.Building;
            buildStartTime = Time.time;
            Debug.Log("Build started");
        }
    }

    public void BuildEnd() {
        robotState = RobotState.Normal;
        Debug.Log("Build ended");
    }

    public void BuildCarrying() {
        Debug.Log("Building placed!!");
        WorldManager.Instance.PlaceStation(carrying.cargo, transform.position + transform.forward);
        Destroy(carrying.gameObject);
        carrying = null;
        BuildEnd();
    }
}
