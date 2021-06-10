using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> spawningEnemies;

    public bool spawnOnStart = false;

    public float spawnTime = 10f;

    public int maxEnemyCount = 3;

    [HideInInspector] public bool currentlySpawning = false;

    void Start()
    {
        if (spawnOnStart) Spawn(spawningEnemies[Random.Range(0, spawningEnemies.Count)], transform.position, transform.rotation);
    }

    void Update() 
    {
        if (!currentlySpawning && transform.childCount < maxEnemyCount) {
            StartCoroutine("WaitSpawn");
        }
    }

    public void Spawn(GameObject enemy, Vector3 spawnPos, Quaternion spawnRot)
    {
        var prefabInstance = Instantiate(enemy, spawnPos, spawnRot);

        prefabInstance.transform.SetParent(transform);
    }

    private IEnumerator WaitSpawn()
    {
        currentlySpawning = true;
        yield return new WaitForSeconds(spawnTime);

        Spawn(spawningEnemies[Random.Range(0, spawningEnemies.Count)], transform.position, transform.rotation);

        currentlySpawning = false;
    }
}
