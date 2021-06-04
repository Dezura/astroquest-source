using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : BaseAI
{
    public float speed = 300f;

    public DetectionArea detectionArea;

    public GameObject thrusterRoot;

    [HideInInspector] public float distanceFromTarget;

    [HideInInspector] public Transform target;

    Dictionary<string, float> thrustersSpeed = new Dictionary<string, float>
    {
        {"max", 100f},
        {"lerpSpeed", 7.5f},
        {"current", 0f}
    };

    void Start()
    {
        GetGlobals();

        entity = GetComponent<Entity>();

        target = g.playerShip.transform;
    }

    void Update() // Overriding this instead of using UpdateAI() to make it simpler when writing enemy AI
    {
        if (entity.isDead) {WhileDead(); return;}

        distanceFromTarget = Vector3.Distance(transform.position, target.position);

        if (detectionArea.overlappingEntities.Contains(g.playerShip.entity)) {WhileInAttackRange();}
        else {WhileOutOfRange();}

        UpdateAI();
        UpdateThrusters();
    }

    public void MoveTowards(Vector3 position)
    {
        AddThrusterForce();
        entity.rigidBody.AddForce((position - transform.position).normalized * speed);
    }

    public void AddThrusterForce()
    {
        thrustersSpeed["current"] = Mathf.Lerp(thrustersSpeed["current"], thrustersSpeed["max"], thrustersSpeed["lerpSpeed"]);
    }

    public void UpdateThrusters()
    {
        foreach (ParticleSystem thruster in thrusterRoot.GetComponentsInChildren<ParticleSystem>())
        {
            ParticleSystem.EmissionModule particleEmission = thruster.emission;
            ParticleSystem.MainModule particleMain = thruster.main;
            
            particleEmission.rateOverTime = thrustersSpeed["current"];
            particleMain.startLifetime = thrustersSpeed["current"]/300f;
        }
        thrustersSpeed["current"] = Mathf.Lerp(thrustersSpeed["current"], 0, thrustersSpeed["lerpSpeed"]);
    }

    // Override these functions in ai scripts as needed

    public virtual void WhileOutOfRange() {}

    public virtual void WhileInAttackRange() {}
}
