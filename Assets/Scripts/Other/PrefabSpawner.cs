using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : Utils
{
    // TODO: Make a custom editor script for this
    // TODO: Also add multi prefab support, alongside spawn chances

    public GameObject prefab;

    public Transform spawnParent;

    public bool autospawn = false; // Implement this later
    public bool spawnOnStart = false;

    void Start()
    {
        if (spawnOnStart) Spawn(transform.position, transform.rotation);
    }

    public void Spawn(Vector3 spawnPos, Quaternion spawnRot)
    {
        var prefabInstance = Instantiate(prefab, spawnPos, spawnRot);

        prefabInstance.transform.SetParent(spawnParent);
    }
}
