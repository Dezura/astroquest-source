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

    void Update() {if (entity.isDead) WhileDead(); else UpdateAI();}

    void FixedUpdate() {if (entity.isDead) WhileDead(); else FixedUpdateAI();}



    // Override these functions in ai scripts as needed

    public virtual void UpdateAI() {}

    public virtual void FixedUpdateAI() {}

    public virtual void AfterHit(GameObject hitSource, float damage, float forceApplied = 0, Vector3? hitPosition = null) // Do not use this directly to apply damage 
    {
        HitFlash();
    }

    public virtual void OnDeath(GameObject hitSource) {} // Keep init type stuff on death here, don't destroy to follow design

    public virtual void WhileDead() {} // Keep ongoing and destroy logic here



    // Extra helper/other type functions

    private IEnumerator RevertFlashAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        foreach (MeshRenderer mesh in transform.GetComponentsInChildren<MeshRenderer>())
        {
            foreach (Material material in mesh.materials)
            {
                material.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    public void HitFlash()
    {
        StopCoroutine("RevertFlashAfter");

        foreach (MeshRenderer mesh in transform.GetComponentsInChildren<MeshRenderer>())
        {
            foreach (Material material in mesh.materials)
            {
                if (!material.IsKeywordEnabled("_EMISSION")) {
                    material.EnableKeyword("_EMISSION");
                }
                material.SetColor("_EmissionColor", new Color(0.2f, 0.2f, 0.2f));
            }
        }

        StartCoroutine("RevertFlashAfter", 0.075f);
    }
}
