using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Projectile : Utils
{
    public AudioSource soundEffect;

    public bool pendingDestroy = false;

    public ParticleSystem destroyExplosion;
    public Transform spawnPoint;

    public float damage;
    public float speed;
    public float lifespan;
    public float sizeMultiplier;
    public float knockbackForce;

    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public MeshRenderer model;
    [HideInInspector] public Vector3 lastPos;

    // Since this script inherits from Utils, I need to override Awake() as it calls GetGlobals(), which doesn't work well with prefabs on Awake()
    void Awake() {}

    void Start() {Init();}

    void Update() {if (pendingDestroy) WhileDestroying(); else UpdateProjectile();}

    void FixedUpdate() {if (pendingDestroy) WhileDestroying(); else FixedUpdateProjectile();}

    private void OnTriggerEnter(Collider collider) {OnHitCollision(collider);}

    public IEnumerator<float> LifespanTimeout() {yield return Timing.WaitForSeconds(lifespan); OnLifespanTimeout();}



    // Override these functions in ai scripts as needed

    public virtual void Init() 
    {
        GetGlobals();

        FixStats();
        
        rigidBody = GetComponent<Rigidbody>();
        model = GetComponentInChildren<MeshRenderer>();

        transform.localScale *= sizeMultiplier;
        
        transform.position += transform.position - spawnPoint.position;

        Timing.RunCoroutine(LifespanTimeout().CancelWith(gameObject));

        soundEffect.volume = g.globalVolume;
        soundEffect.Play();
    }

    public virtual void FixStats() {}

    public virtual void UpdateProjectile() 
    {
        lastPos = transform.position;
    }
    
    public virtual void FixedUpdateProjectile() {}

    public virtual void OnHitCollision(Collider collider) {}

    public virtual void OnLifespanTimeout()
    {
        OnDestroy();
    }

    public virtual void OnDestroy()
    {
        pendingDestroy = true;

        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>()) Destroy(mesh);
        Destroy(GetComponentInChildren<MeshCollider>());
        Destroy(GetComponent<Rigidbody>());

        destroyExplosion.Play();
    }

    public virtual void WhileDestroying()
    {
        if (!destroyExplosion.isPlaying) Destroy(gameObject);
    }



    // Extra helper/other type functions

    public Transform GetClosestTarget()
    {
        switch (tag)
        {
            case "Player Projectile":
                EnemyAI closestEnemy = GetClosestEnemy();

                if (closestEnemy) return closestEnemy.transform; 

                break;
            case "Enemy Projectile":
                return g.playerShip.transform;
        }

        return null;
    }

    public EnemyAI GetClosestEnemy(Transform exludeTransform = null)
    {
        EnemyAI closestEnemy = null;
        foreach (EnemyAI enemy in g.enemySpawn.GetComponentsInChildren<EnemyAI>())
        {
            if (exludeTransform == enemy.transform) continue;
            if (!closestEnemy) {closestEnemy = enemy; continue;}

            if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, closestEnemy.transform.position)) closestEnemy = enemy;
        }

        return closestEnemy;
    }
}
