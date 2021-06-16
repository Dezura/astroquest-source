using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class EnemySpawner : MonoBehaviour
{
    public Transform playerTransform;
    public TimerAndScore timerAndScore;

    public float spawnDistance = 75f;
    public float closePass = 35f;

    public float gracePeriodTime = 10f;

    public List<GameObject> spawningEnemies;

    [HideInInspector] public float spawnTime = 10f;
    [HideInInspector] public int maxEnemyCount = 3;

    [HideInInspector] public bool currentlySpawning = false;

    void Update() 
    {
        maxEnemyCount = 2 + (int) timerAndScore.currentTime.TotalMinutes;
        spawnTime = Mathf.Max(1, 15 - ((float) timerAndScore.currentTime.TotalMinutes) * 3);

        if (!currentlySpawning && transform.childCount < maxEnemyCount && (float) timerAndScore.currentTime.Seconds >= gracePeriodTime) {
            Timing.RunCoroutine(WaitSpawn().CancelWith(gameObject));
        }
    }

    public void Spawn(GameObject enemy, Vector3 spawnPos, Quaternion spawnRot)
    {
        var enemyInstance = Instantiate(enemy, spawnPos, spawnRot, transform);
    }

    public IEnumerator<float> WaitSpawn()
    {
        currentlySpawning = true;
        float actualSpawnTime = spawnTime;
        if (transform.childCount == 0) actualSpawnTime /= 4f;
        if (transform.childCount <= 4) actualSpawnTime /= 2f;
        yield return Timing.WaitForSeconds(actualSpawnTime);

        Vector3 randPosOffset = Random.onUnitSphere * Random.Range(closePass, spawnDistance);
        Spawn(spawningEnemies[Random.Range(0, spawningEnemies.Count)], playerTransform.position + randPosOffset, transform.rotation);

        currentlySpawning = false;
    }
}
