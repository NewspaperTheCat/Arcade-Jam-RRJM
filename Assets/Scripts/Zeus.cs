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
    [SerializeField] float smiteCooldown;
    [SerializeField] LineRenderer lightningArc;
    [SerializeField] ParticleSystem smiteBurst;
    [SerializeField] Material targetMaterial; // used to display if charge is ready
    [SerializeField] GameObject SFX;
    [SerializeField] AudioClip thunderClip;

    bool smiteReady = true;

    void Start() {
        InputManager.inst.smite.AddListener(Smite);
        lightningArc.enabled = false;

        targetMaterial.SetVector("_EmissionColor", new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
        smiteReady = true;
    }

    // Update is called once per frame
    void Update() {
        // Gather Player Input
        velocity += InputManager.inst.GetZeusMovement() * playerInfluence * Time.deltaTime;
        transform.LookAt(transform.position + new Vector3(velocity.x, 0, velocity.y));

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

        // Particle system handles its own flashing
        smiteBurst.Play();

        // List of collided stuff
        List<Transform> collisions = new List<Transform>();

        // ready target flashing
        bool flashOn = true;
        Vector4 off = new Vector4(.0f, .0f, .0f, 1.0f);
        Vector4 on = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

        // Retrigger randomly over duration
        float maxDuration = .625f;
        float duration = maxDuration;
        while (duration > 0) {

            // Get Duration
            float delay = Random.Range(0.1f, 0.3f);
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

            // Flash target
            if (flashOn) targetMaterial.SetVector("_EmissionColor", off);
            else targetMaterial.SetVector("_EmissionColor", on);
            flashOn = !flashOn;

            // Play thunder
            PlayThunder(delay, (duration + delay) / maxDuration);

            yield return new WaitForSeconds(delay);
        }

        // Display the cooldown
        targetMaterial.SetVector("_EmissionColor", off);
        lightningArc.enabled = false;

        yield return new WaitForSeconds(smiteCooldown);

        // Return to ready
        targetMaterial.SetVector("_EmissionColor", on);
        smiteReady = true;
    }

    public void Spawn() {
        return;
    }
 
    private void PlayThunder(float duration, float remaining) {
        AudioSource sfx = Instantiate(SFX, transform.position + Vector3.up * 3, Quaternion.identity).GetComponent<AudioSource>();
        sfx.clip = thunderClip;
        Destroy(sfx.gameObject, thunderClip.length + .5f);

        sfx.pitch = (.3f - duration) * 2f + .8f + remaining * .9f;
        sfx.volume = remaining;
        sfx.Play();
    }
}
