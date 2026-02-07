using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    //public List<Enemy> enemies = new List<Enemy>();
    //[HideInInspector] public List<GameObject> enemiesToSpawn = new List<GameObject>();
    //public float spawnRadius;
    //private float spawnInterval;
    //private bool waveReadySpawn = false;
    //private float spawnTimer;
    //private bool coolDownStarted = false;
    //[SerializeField] private float initialCoolDown;
    //[HideInInspector] public float currentCoolDown;
    //[SerializeField] private int initialWaveDuration;
    //[SerializeField] private int increaseWaveDuration;
    //private int currentWaveDuration;
    //public int currWave;
    //[SerializeField] private int initialEnemyWaveCost;
    //[SerializeField] private int increaseEnemyWaveCost;
    //private int currentEnemyWaveCost;
    //[SerializeField] private int staringWave;


    //private Transform centerOfSpawn;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, spawnRadius);
    //}

    //void Start()
    //{
    //    currentWaveDuration = initialWaveDuration;
    //    currWave = staringWave;
    //    centerOfSpawn = gameObject.transform;
    //    currentEnemyWaveCost = initialEnemyWaveCost;
    //    GenerateWave();
    //}

    //private void GenerateWave()
    //{

    //    int waveCost = currentEnemyWaveCost;
    //    currentCoolDown = initialCoolDown;
    //    GenerateNPCS(waveCost);
    //    Debug.Log("Current wave: " + currWave + "; WaveValue:  " + currentEnemyWaveCost + "; Next Wave in: " + currentWaveDuration);
    //    spawnInterval = currentWaveDuration / enemiesToSpawn.Count;
    //}

    //private void GenerateNPCS(int waveCost)
    //{
    //    List<GameObject> generatedEnemies = new List<GameObject>();
    //    while (waveCost > 0)
    //    {
    //        int randEnemyId = Random.Range(0, enemies.Count);
    //        int RandEnemyCost = enemies[randEnemyId].cost;

    //        if (waveCost - RandEnemyCost >= 0)
    //        {
    //            generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
    //            waveCost -= RandEnemyCost;
    //        }
    //        else if (waveCost <= 0)
    //        {
    //            break;
    //        }
    //    }
    //    enemiesToSpawn.Clear();
    //    enemiesToSpawn = generatedEnemies;
    //}

    //private void Update()
    //{
    //    if (GameManager.Instance.getGameOver()) return;

    //    if (waveReadySpawn)
    //    {
    //        if (!coolDownStarted)
    //        {
    //            //Debug.Log("Starting Cooldown: " + currWave);
    //            coolDownStarted = true;
    //            StartCoroutine(WaveCoolDown());
    //        }
    //    }
    //    else if (spawnTimer <= 0)
    //    {
    //        if (enemiesToSpawn.Count > 0)
    //        {
    //            Vector2 spawnPosition = GetRandomSpawnPosition();
    //            Quaternion spawnRotation = Quaternion.identity;
    //            Instantiate(enemiesToSpawn[0], spawnPosition, spawnRotation);
    //            enemiesToSpawn.RemoveAt(0);
    //            spawnTimer = spawnInterval;
    //        }
    //        else
    //        {
    //            waveReadySpawn = true;
    //        }
    //    }
    //    else
    //    {
    //        spawnTimer -= Time.deltaTime;
    //    }
    //}

    //private IEnumerator WaveCoolDown()
    //{
    //    //Debug.Log("Cooldown started, waiting this wave is: " + currWave);
    //    yield return new WaitForSeconds(currentCoolDown);
    //    //Debug.Log("Cooldown complete, setting up next wave: " + currWave);

    //    currentWaveDuration = initialWaveDuration + (increaseWaveDuration * currWave);
    //    currentEnemyWaveCost = initialEnemyWaveCost + (increaseEnemyWaveCost * currWave);
    //    currWave++;


    //    waveReadySpawn = false;
    //    coolDownStarted = false;
    //    GenerateWave();
    //}

    //private Vector3 GetRandomSpawnPosition()
    //{
    //    Vector2 randomCircle = Random.insideUnitCircle.normalized;
    //    float spawnDistance = spawnRadius + Random.Range(-1f, 1f);

    //    Vector3 direction = new Vector3(randomCircle.x, 0f, randomCircle.y);

    //    Vector3 spawnPosition = centerOfSpawn.transform.position + direction * spawnDistance;

    //    return spawnPosition;
    //}
}
