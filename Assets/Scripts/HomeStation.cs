using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeStation : Station
{
    bool landed;
    [SerializeField] ParticleSystem landingParticles;
    float velocity;
    const float gravity = 10;

    [SerializeField] Zeus zeusPrefab;
    [SerializeField] Robot robotPrefab;

    override protected void Start() {
        base.Start();

        // Intro Sequence
        landed = false;
        transform.position = Vector3.up * 50;
        velocity = -10;
    }

    void Update() {
        if (!landed) {
            velocity += -gravity * Time.deltaTime;
            transform.position += Vector3.up * velocity * Time.deltaTime;
            landingParticles.transform.position = Vector3.zero;
            if (transform.position.y <= 0) {
                transform.position = Vector3.zero;
                landed = true;
                landingParticles.Play();
                SpawnPlayers();
            }
        }
    }

    public override void Die() {
        base.Die();
        GameManager.Instance.SetGameOver();
    }

    private void SpawnPlayers() {
        Zeus zeus = Instantiate(zeusPrefab, Vector3.zero, Quaternion.identity);
        Robot robot = Instantiate(robotPrefab, Vector3.zero, Quaternion.identity);
        zeus.Spawn();
        robot.Spawn();
    }
}
