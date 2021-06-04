using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    [HideInInspector] public Collider areaCollider;

    [HideInInspector] public List<Collider> overlappingColliders;
    [HideInInspector] public List<Entity> overlappingEntities;

    void Start()
    {
        areaCollider = GetComponent<SphereCollider>();
    }

    void OnTriggerEnter(Collider collider)
    {
        overlappingColliders.Add(collider);

        EntityForwarder entityForwarder;
        if (collider.TryGetComponent<EntityForwarder>(out entityForwarder)) {
            overlappingEntities.Add(entityForwarder.targetEntity);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        overlappingColliders.Remove(collider);

        EntityForwarder entityForwarder;
        if (collider.TryGetComponent<EntityForwarder>(out entityForwarder)) {
            overlappingEntities.Remove(entityForwarder.targetEntity);
        }
    }
}
