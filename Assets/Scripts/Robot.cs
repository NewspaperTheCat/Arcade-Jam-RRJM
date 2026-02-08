using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Robot : MonoBehaviour
{
    [SerializeField] float speed;

    Vector2 velocity;
    [SerializeField] float hopAmount;
    float hopVelocity = 0;

    // Miscellaneous: for intro sequence
    bool firstCharge;

    [Header("Charge")]
    float charge = 1; // max charge = 1, empty = 0
    [SerializeField] float noChargeAmount; // max charge = 1, empty = 0
    Image chargeBar;
    [SerializeField] Material robotMaterial;
    [SerializeField] float passiveDecay;

    // Item related values
    [Header("Items/Stations")]
    Item carrying = null;
    [SerializeField] float buildTime;
    float buildStartTime = 0;
    [SerializeField] ParticleSystem buildParticles;

    public enum RobotState
    {
        Normal,
        NoPower,
        Building
    }

    private RobotState robotState;

    void Start() {
        chargeBar = GameObject.FindWithTag("ChargeBar").GetComponent<Image>();

        InputManager.inst.drop.AddListener(DropItem);
        InputManager.inst.buildStart.AddListener(BuildStart);
        InputManager.inst.buildEnd.AddListener(BuildEnd);

        // stupid work around to set duration
        var main = buildParticles.main;
        main.duration = buildTime;

        firstCharge = false;
    }

    // Update is called once per frame
    void Update() {
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        switch (robotState)
        {
            case RobotState.Normal:
                NormalUpdate();
                break;
            case RobotState.NoPower:
                transform.Rotate(Vector3.forward * (180 - transform.localEulerAngles.z) * Time.deltaTime);
                float modelHeight = 1.25f;
                transform.position = new Vector3(transform.position.x, transform.localEulerAngles.z / 180.0f * modelHeight, transform.position.z);

                // apply any velocity with heavy fall off
                transform.position += new Vector3(velocity.x, 0, velocity.y) * Time.deltaTime;
                velocity -= velocity * Time.deltaTime / .5f;
                break;
            case RobotState.Building:
                if (Time.time - buildStartTime >= buildTime) {
                    BuildCarrying();
                } 
                break;
        }

        if (robotState != RobotState.NoPower) {
            transform.position += Vector3.up * hopVelocity * Time.deltaTime;
            hopVelocity -= Time.deltaTime * 10f;
            if (transform.position.y < 0) {
                hopVelocity = 0;
                transform.position -= Vector3.up * transform.position.y;
            }
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
            velocity = Vector2.zero;
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

        if (robotState == RobotState.NoPower) {
            transform.position -= Vector3.up * transform.position.y;
            transform.Rotate(Vector3.forward * (-transform.localEulerAngles.z));
            robotState = RobotState.Normal;
        }

        // hop into air
        hopVelocity = Mathf.Max(hopAmount, hopVelocity + hopAmount);

        // Trigger first charge
        if (!firstCharge) {
            firstCharge = true;
            EnemySpawner.inst.enabled = true;
        }
    }

    private void UpdateChargeMeter() {
        chargeBar.fillAmount = charge;
        robotMaterial.SetVector("_EmissionColor", new Vector4(charge * 2, charge * 2, charge, 1.0f));
    }

    public bool PickUpItem(Item item) {
        if (carrying != null) return false;

        carrying = item;
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.forward * 1.125f;
        item.SetAnchor();
        return true;
    }

    public void DropItem() {
        if (carrying != null) {
            carrying.Drop();
            carrying = null;
        }
    }

    public void BuildStart() {
        if (carrying != null && robotState == RobotState.Normal) {
            robotState = RobotState.Building;
            buildStartTime = Time.time;
            buildParticles.Play();
            Debug.Log("Build started");
        }
    }

    public void BuildEnd() {
        if (robotState == RobotState.Building) {
            robotState = RobotState.Normal;
            buildParticles.Stop();
            Debug.Log("Build ended");
        }
    }

    public void BuildCarrying() {
        Debug.Log("Building placed!!");
        WorldManager.Instance.PlaceStation(carrying.cargo, transform.position - Vector3.up * transform.position.y + transform.forward);
        Destroy(carrying.gameObject);
        carrying = null;
        BuildEnd();
    }

    public void Spawn() {
        charge = 0;
        robotState = RobotState.NoPower;
        float angle = Random.Range(0, 2 * Mathf.PI);
        velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 10f; // shoot off in a random direction
    }
}
