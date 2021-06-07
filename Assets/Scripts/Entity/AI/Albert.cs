using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Albert : BaseAI
{  
    public bool angry = false;

    public float albertRespawnTime = 3f;
    
    void Start()
    {
        GetGlobals();

        entity = GetComponent<Entity>();
    }

    public override void UpdateAI()
    {
        if (angry) {
            transform.LookAt(g.playerShip.transform, g.playerShip.transform.up);
            transform.Rotate(0, 180, 0);

            entity.rigidBody.AddRelativeForce(new Vector3(0, 0, -100), ForceMode.Force);

            // The way these values are changed is really inefficient, but it won't matter that much as this is just a test entity
            GetComponentInChildren<TextMeshPro>().text = "(ò_ó)";
            GetComponentInChildren<TextMeshPro>().color = new Color(1, 0, 0);
            GetComponentInChildren<Light>().color = new Color(1, 0, 0);
            GetComponent<MeshRenderer>().material.SetColor("_BaseColor", new Color(1, 0, 0));
            GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(0.125f, 0, 0));
        }
    }

    public override void OnHit(GameObject hitSource, float damage, float forceApplied = 0)
    {
        base.OnHit(hitSource, damage, forceApplied);
        
        if (!angry) {angry = true; GetComponentInChildren<ParticleSystem>().Play();}

        Debug.Log("Albert took " + damage + " Damage And Has " + entity.hp["current"] + " Health Remaining!");
    }

    public override void OnDeath(GameObject hitSource)
    {
        GetComponentInChildren<TextMeshPro>().text = "(x_x)";
        GetComponentInChildren<TextMeshPro>().color = new Color(0.32f, 0.32f, 0.32f);
        GetComponentInChildren<Light>().color = new Color(0.32f, 0.32f, 0.32f);
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", new Color(0.32f, 0.32f, 0.32f));
        GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(0, 0, 0));
        GetComponentInChildren<ParticleSystemRenderer>().material.SetColor("_BaseColor", new Color(1, 0, 0));
        GetComponentInChildren<ParticleSystemRenderer>().material.SetColor("_EmissionColor", new Color(0.125f, 0, 0));
        GetComponentInChildren<ParticleSystemRenderer>().trailMaterial = GetComponentInChildren<ParticleSystemRenderer>().material;

        GetComponentInChildren<ParticleSystem>().Play();

        entity.rigidBody.AddExplosionForce(500, hitSource.transform.position, 1f, 0, ForceMode.Impulse);
        
        StartCoroutine("RespawnAlbert");

        Debug.Log("Albert is Dead! You monster!");
    }

    private IEnumerator RespawnAlbert()
    {
        yield return new WaitForSeconds(albertRespawnTime);
        Transform spawner = transform.root.Find("Environmentals/Albert Spawner");
        spawner.GetComponent<PrefabSpawner>().Spawn(spawner.position, spawner.rotation);
    }
}
