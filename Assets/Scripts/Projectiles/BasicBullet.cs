using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : Utils
{
    public bool pendingDestroy = false;
    ParticleSystem particleEmitter;
    Vector3 lastPos;

    public float damage;
    public float speed;
    public float lifespan;
    public float sizeMultiplier;
    public float knockbackForce;

    // Since this script inherits from Utils, I need to override Awake() as it calls GetGlobals(), which doesn't work well with prefabs on Awake()
    void Awake() {}

    void Start() 
    {
        GetGlobals();
        
        particleEmitter = GetComponentInChildren<ParticleSystem>();

        GetComponent<Rigidbody>().velocity = transform.rotation * -(Vector3.up * speed);
        StartCoroutine("LifespanTimeout");
    }

    void FixedUpdate() 
    {
        lastPos = transform.position;
        if (pendingDestroy && particleEmitter.isPlaying == false) {
            // TODO: Switch to pulling instead of destroying for better optimization
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        RaycastHit hit;
        if(Physics.Raycast(lastPos, (transform.position - lastPos), out hit, Vector3.Distance(lastPos, transform.position), g.layerMasks["Player Bullet"])) {
            // Makes the particle effects look a lot better with high speed bullets, and also makes sure they don't clip too much
            // This is also used to accurately apply knockback to entities
            transform.position = hit.point;
        }

        Entity entity;
        if (collider.TryGetComponent<Entity>(out entity)) {
            entity.OnHit(gameObject, damage, knockbackForce);
        }

        QueueDestroy();
    }

    private IEnumerator LifespanTimeout()
    {
        yield return new WaitForSeconds(lifespan);
        QueueDestroy();
    }

    public void QueueDestroy()
    {
        pendingDestroy = true;

        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(GetComponent<Rigidbody>());

        particleEmitter.Play();
    }
}
