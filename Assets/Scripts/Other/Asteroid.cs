using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public AsteroidSpawner spawner;

    public Transform playerTransform;

    public float despawnDistance;

    void Update()
    {
        if (Vector3.Distance(transform.position, playerTransform.transform.position) > despawnDistance) spawner.Respawn(this);
    }
}
