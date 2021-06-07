using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : BaseAI
{
    public GunManager gunManager;
    public MeshRenderer model;

    public float speed = 300f;

    public float contactDamage = 200f;

    public DetectionArea detectionArea;
    public DetectionArea closeDetectionArea;

    public GameObject thrusterRoot;

    [HideInInspector] public float distanceFromTarget;

    [HideInInspector] public Transform target;

    [HideInInspector] public float playerAvoidanceDistance = 20f;
    [HideInInspector] public float enemyAvoidanceDistance = 20f;
    [HideInInspector] public float mouseAvoidanceDistance = 250f;

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

        Init();
    }

    void Update() // Overriding this instead of using UpdateAI() to make it simpler when writing enemy AI
    {
        if (entity.isDead) {WhileDead(); return;}

        distanceFromTarget = Vector3.Distance(transform.position, target.position);

        if (closeDetectionArea.overlappingEntities.Contains(g.playerShip.entity)) {WhileInCloseRange();}
        else if (detectionArea.overlappingEntities.Contains(g.playerShip.entity)) {WhileInAttackRange();}
        else {WhileOutOfRange();}

        UpdateAI();
        UpdateThrusters();

        // TODO: Add healthbar
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

    public void AvoidMouse()
    {        
        if (Vector2.Distance(g.mainCamera.WorldToScreenPoint(transform.position), g.virtualMouse.vMousePosition) <= mouseAvoidanceDistance) { // If near mouse
            Vector2 dirFromMouse = (((Vector2) g.mainCamera.WorldToScreenPoint(transform.position)) - g.virtualMouse.vMousePosition).normalized; // Calculate direction from the mouse to the enemy relative to screenspace
            Vector3 moveDir = g.mainCamera.transform.rotation * new Vector3(dirFromMouse.x, dirFromMouse.y, 0); // Convert the direction to a Vector3 in worldspace

            entity.rigidBody.AddForce(moveDir * speed); // Then move using the new moveDir
        }
    }

    public void AvoidNearbyContactFrom(string from)
    {
        foreach (Entity detectedEntity in closeDetectionArea.overlappingEntities) { // First avoid colliding with enemies and keep distance (This also give an illusion of a more complex groupthinking type AI)
            if (!detectedEntity) continue; // If the entity is dead, but it's still in the overlapping list, skip to next iteration
            EnemyAI enemy;
            ShipMain playerShip;

            if (detectedEntity.TryGetComponent<EnemyAI>(out enemy) && from == "Enemy") {
                if (Vector3.Distance(transform.position, enemy.transform.position) <= enemyAvoidanceDistance) {
                    Vector3 moveDir = (transform.position - enemy.transform.position).normalized;

                    entity.rigidBody.AddForce(moveDir * speed);
                }
            }

            else if (detectedEntity.TryGetComponent<ShipMain>(out playerShip) && from == "Player") {
                if (Vector3.Distance(transform.position, playerShip.transform.position) <= playerAvoidanceDistance) {
                    Vector3 moveDir = (transform.position - playerShip.transform.position).normalized;

                    entity.rigidBody.AddForce(moveDir * speed);
                }
            }
        }
    }

    public override void OnDeath(GameObject hitSource)
    {
        base.OnDeath(hitSource);

        // TODO: Add explosion and random flying scrap
        // TODO: Add screenshake on death
        // TODO: Give points to score on death
    }

    public override void WhileDead()
    {
        Destroy(gameObject);
    }

    // Override these functions in ai scripts as needed

    public virtual void Init() {}

    public virtual void WhileOutOfRange() {}

    public virtual void WhileInAttackRange() {}

    public virtual void WhileInCloseRange() {}
}
