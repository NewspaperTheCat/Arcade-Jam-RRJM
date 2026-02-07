using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Zeus : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float playerInfluence;
    [SerializeField] float maxSpeed;
    [SerializeField] float friction; // percent of total speed lost in a second

    Vector2 velocity;

    [Header("Smite")]
    [SerializeField] float smiteRadius;

    void Start() {
        InputManager.inst.smite.AddListener(Smite);
    }

    // Update is called once per frame
    void Update() {
        // Gather Player Input
        velocity += InputManager.inst.GetZeusMovement() * playerInfluence * Time.deltaTime;

        // Apply Friction
        velocity -= velocity * friction * Time.deltaTime;

        // Clamp to max
        if (velocity.magnitude > maxSpeed)
            velocity = velocity.normalized * maxSpeed;

        // Apply velocity
        transform.position += new Vector3(velocity.x, 0, velocity.y) * Time.deltaTime;

        // Clamp position
        if (transform.position.magnitude > 10) {
            transform.position = transform.position.normalized * 10;
        }
    }

    // Casts a circle overlap cast and effects each appropriately
    public void Smite() {
        Collider[] hits = Physics.OverlapSphere(Vector3.up * .5f + transform.position, smiteRadius, ~LayerMask.GetMask("Ground"));
        foreach(Collider col in hits) {
            if (col.transform.GetComponent<Robot>() != null) {
                col.transform.GetComponent<Robot>().Recharge();
            }
            if (col.transform.GetComponent<Grub>() != null)
            {
                col.transform.GetComponent<Grub>().TakeDamage(100);
            }
        }
    }
}
