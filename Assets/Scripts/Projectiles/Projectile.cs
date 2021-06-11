using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Utils
{
    public bool pendingDestroy = false;

    public ParticleSystem destroyExplosion;
    public Transform spawnPoint;

    public float damage;
    public float speed;
    public float lifespan;
    public float sizeMultiplier;
    public float knockbackForce;

    [HideInInspector] public Vector3 lastPos;

    // Since this script inherits from Utils, I need to override Awake() as it calls GetGlobals(), which doesn't work well with prefabs on Awake()
    void Awake() {}

    void Start() {Init();}

    void Update() {if (pendingDestroy) WhileDestroying(); else UpdateProjectile();}

    void FixedUpdate() {if (pendingDestroy) WhileDestroying(); else FixedUpdateProjectile();}

    private void OnTriggerEnter(Collider collider) {OnHitCollision(collider);}

    private IEnumerator LifespanTimeout() {yield return new WaitForSeconds(lifespan); OnLifespanTimeout();}



    // Override these functions in ai scripts as needed

    public virtual void Init() 
    {
        GetGlobals();
        
        destroyExplosion = GetComponentInChildren<ParticleSystem>();

        transform.localScale *= sizeMultiplier;
        
        transform.position -= transform.position - spawnPoint.position;

        StartCoroutine("LifespanTimeout");
    }

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
}
