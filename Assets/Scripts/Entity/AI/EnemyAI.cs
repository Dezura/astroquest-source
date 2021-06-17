using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : BaseAI
{
    public GunManager gunManager;

    public string startingGun;
    public string startingProjectile;

    public float speed = 300f;

    public float contactDamage = 200f;

    public DetectionArea detectionArea;
    public DetectionArea closeDetectionArea;

    public GameObject thrusterRoot;

    public ParticleSystem destroyExplosion;
    public AudioSource explosionSFX;
    public AudioSource hitSFX;

    [HideInInspector] public float distanceFromTarget;

    [HideInInspector] public Transform target;

    [HideInInspector] public float playerAvoidanceDistance = 20f;
    [HideInInspector] public float enemyAvoidanceDistance = 20f;
    [HideInInspector] public float mouseAvoidanceDistance = 500f;

    [HideInInspector] public string currentDetection = "None"; // None; Far; Close;

    Dictionary<string, float> thrustersSpeed = new Dictionary<string, float>
    {
        {"max", 100f},
        {"lerpSpeed", 7.5f},
        {"current", 0f}
    };

    ParticleSystem[] thrusters;

    void Start() {Init();}



    // Override these functions in ai scripts as needed

    public virtual void Init() 
    {
        GetGlobals();

        entity = GetComponent<Entity>();

        target = g.playerShip.transform;

        gunManager.SetCurrentGun(startingGun, startingProjectile);
        gunManager.AimGunPoints(g.playerShip.transform.position, transform.up, true);
        gunManager.currentGun.projectileSpeed = Mathf.Min(180, 80 + (20 * ((float) g.timerAndScore.currentTime.TotalMinutes)));

        thrusters = thrusterRoot.GetComponentsInChildren<ParticleSystem>();
    }

    public override void UpdateAI()
    {
        base.UpdateAI();

        distanceFromTarget = Vector3.Distance(transform.position, target.position);

        UpdateThrusters();

        // TODO: Add healthbar
    }

    public override void FixedUpdateAI()
    {
        base.FixedUpdateAI();

        if (closeDetectionArea.overlappingEntities.Contains(g.playerShip.entity)) currentDetection = "Close";
        else if (detectionArea.overlappingEntities.Contains(g.playerShip.entity)) currentDetection = "Far";
        else currentDetection = "None";
    }

    public override void AfterHit(GameObject hitSource, float damage, float forceApplied = 0, Vector3? hitPosition = null)
    {
        base.AfterHit(hitSource, damage, forceApplied, hitPosition);

        hitSFX.volume = g.globalVolume;
        hitSFX.Play();
    }

    public override void OnDeath(GameObject hitSource)
    {
        base.OnDeath(hitSource);

        destroyExplosion.Play();
        destroyExplosion.GetComponent<Light>().enabled = true;
        explosionSFX.volume = g.globalVolume;
        explosionSFX.Play();

        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>()) Destroy(mesh);
        Destroy(thrusterRoot);
        Destroy(GetComponentInChildren<MeshCollider>());
        Destroy(GetComponent<Rigidbody>());

        g.playerCamera.ApplyScreenShake(25f);

        g.timerAndScore.AddScore(Random.Range(40, 50) * (1 + (int) g.timerAndScore.currentTime.TotalMinutes / 10f) * 10);
    }

    public override void WhileDead()
    {
        base.WhileDead();

        if (!destroyExplosion.isPlaying && !explosionSFX.isPlaying) Destroy(gameObject);
    }



    // Extra helper/other type functions

    public void MoveTowards(Vector3 position)
    {
        AddThrusterForce();
        entity.rigidBody.AddForce((position - transform.position).normalized * speed * 12);
    }

    public void AddThrusterForce()
    {
        thrustersSpeed["current"] = Mathf.Lerp(thrustersSpeed["current"], thrustersSpeed["max"], thrustersSpeed["lerpSpeed"]);
    }

    public void UpdateThrusters()
    {
        foreach (ParticleSystem thruster in thrusters)
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
        if (Vector2.Distance(g.playerCamera.cam.WorldToScreenPoint(transform.position), g.virtualMouse.vMousePosition[1]) <= mouseAvoidanceDistance) { // If near mouse
            Vector2 dirFromMouse = (((Vector2) g.playerCamera.cam.WorldToScreenPoint(transform.position)) - g.virtualMouse.vMousePosition[1]).normalized; // Calculate direction from the mouse to the enemy relative to screenspace
            Vector3 moveDir = g.playerCamera.cam.transform.rotation * new Vector3(dirFromMouse.x, dirFromMouse.y, 0); // Convert the direction to a Vector3 in worldspace

            entity.rigidBody.AddForce(moveDir * speed * 12); // Then move using the new moveDir
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

                    entity.rigidBody.AddForce(moveDir * speed * 12);
                }
            }

            else if (detectedEntity.TryGetComponent<ShipMain>(out playerShip) && from == "Player") {
                if (Vector3.Distance(transform.position, playerShip.transform.position) <= playerAvoidanceDistance) {
                    Vector3 moveDir = (transform.position - playerShip.transform.position).normalized;

                    entity.rigidBody.AddForce(moveDir * speed * 12);
                }
            }
        }
    }
}
