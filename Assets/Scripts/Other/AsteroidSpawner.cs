using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public Transform playerTransform;

    public int maxAsteroidAmount = 100;

    public float spawnDistance = 250f;
    public float despawnDistance = 250f;

    public float closePass = 100f;

    public float randScaleMin = 0.5f;
    public float randScaleMax = 2f;

    void SpawnChunk() // Called this once in the editor, to randomly spawn a chunk of asteroids in the scene 
    {
        for (var i = 0; i < maxAsteroidAmount; i++)
        {
            Vector3 randPosOffset = Random.onUnitSphere * Random.Range(closePass, spawnDistance);

            Asteroid newAsteroid = Instantiate(asteroidPrefab, playerTransform.position + randPosOffset, Random.rotation, transform).GetComponent<Asteroid>();
            newAsteroid.spawner = this;
            newAsteroid.playerTransform = playerTransform;
            newAsteroid.despawnDistance = despawnDistance;

            newAsteroid.transform.localScale = new Vector3(Random.Range(randScaleMin, randScaleMax), Random.Range(randScaleMin, randScaleMax), Random.Range(randScaleMin, randScaleMax));
        }
    }

    public void Respawn(Asteroid asteroid)
    {
        Vector3 randPosOffset = Random.onUnitSphere * Random.Range(closePass, spawnDistance);
        asteroid.transform.position = playerTransform.position + randPosOffset;
    }
}
