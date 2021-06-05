using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a base class all entities inherit from

public class BaseAI : Utils
{
    [HideInInspector] public Entity entity;

    void Awake() {} // This overrides the Awake() in Utils

    void Start() // I set this to Start() instead of Awake() as most entities are instanced prefabs, which don't seem to work well with Awake()
    {
        GetGlobals();

        entity = GetComponent<Entity>();
    }

    void Update()
    {
        if (entity.isDead) {WhileDead(); return;}

        UpdateAI();
    }

    void FixedUpdate()
    {
        if (entity.isDead) {WhileDead(); return;}

        FixedUpdateAI();
    }


    // Override these functions in ai scripts as needed

    public virtual void UpdateAI() {}

    public virtual void FixedUpdateAI() {}

    public virtual void OnHit(GameObject hitSource, float damage, float forceApplied = 0) {}

    public virtual void OnDeath(GameObject hitSource) {} // Keep init type stuff on death here, don't destroy to follow design

    public virtual void WhileDead() {} // Keep ongoing and destroy logic here
}
