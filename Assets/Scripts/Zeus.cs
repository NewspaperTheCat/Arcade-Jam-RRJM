using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
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
    [SerializeField] LineRenderer lightningArc;
    bool smiteReady = true;

    void Start() {
        InputManager.inst.smite.AddListener(Smite);
        lightningArc.enabled = false;
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
        if (!smiteReady) return; // currently lightning-y, so wait

        StartCoroutine("SmiteAnimation");
    }

    // Zip Zap Zop Animation
    IEnumerator SmiteAnimation() {
        lightningArc.enabled = true;
        smiteReady = false;
        
        // List of collided stuff
        List<Transform> collisions = new List<Transform>();

        // Retrigger randomly over duration
        float duration = .625f;
        while (duration > 0) {
            // Get Duration
            float delay = Random.Range(0.1f, 0.475f);
            if (duration < delay) delay = duration;
            duration -= delay;

            // Set Lightning arc
            Vector3[] points = new Vector3[4];
            lightningArc.GetPositions(points);
            points[0] = Vector3.up * 4.0f;
            points[1] = Vector3.up * (2.666f + Random.Range(-.5f, .5f)) + Vector3.right * Random.Range(-.5f, .5f);
            points[2] = Vector3.up * (1.333f + Random.Range(-.5f, .5f)) + Vector3.right * Random.Range(-.5f, .5f);
            points[3] = Vector3.zero;
            lightningArc.SetPositions(points);


            // Detect new potential collision
            Collider[] hits = Physics.OverlapSphere(Vector3.up * .5f + transform.position, smiteRadius, ~LayerMask.GetMask("Ground"), QueryTriggerInteraction.Collide);
            foreach(Collider col in hits) {
                if (collisions.Contains(col.transform)) continue; // skip this one
                // otherwise
                collisions.Add(col.transform);

                if (col.transform.GetComponent<Robot>() != null) {
                    col.transform.GetComponent<Robot>().Recharge();
                }
                if (col.transform.GetComponent<Grub>() != null)
                {
                    col.transform.GetComponent<Grub>().TakeDamage(100);
                }
                if (col.transform.GetComponent<Station>() != null) {
                    col.transform.GetComponent<Station>().OnHit();
                }
            }

            yield return new WaitForSeconds(delay);
        }

        lightningArc.enabled = false;

        yield return new WaitForSeconds(.5f);

        smiteReady = true;
    }
}
