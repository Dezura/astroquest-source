using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    public bool pendingDestroy = false;
    ParticleSystem particleEmitter;

    public float damage;
    public float speed;
    public float lifespan;

    void Start() 
    {
        particleEmitter = GetComponentInChildren<ParticleSystem>();

        GetComponent<Rigidbody>().velocity = transform.rotation * -(Vector3.up * speed);
        StartCoroutine("LifespanTimeout");
    }

    void FixedUpdate() 
    {
        if (pendingDestroy && particleEmitter.isPlaying == false) {
            // TODO: Switch to pulling instead of destroying for better optimization
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // TODO: Call OnHit() on object hit if it's an entity

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
