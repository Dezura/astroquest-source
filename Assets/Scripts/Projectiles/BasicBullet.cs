using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    public bool pendingDestroy = false;
    ParticleSystem particleEmitter;
    Vector3 lastPos;

    public float damage;
    public float speed;
    public float lifespan;
    public float sizeMultiplier;

    void Start() 
    {
        particleEmitter = GetComponentInChildren<ParticleSystem>();

        //particleEmitter.transform.localScale = Vector3.one * sizeMultiplier/2f;
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

        RaycastHit hit;
        if(Physics.Raycast(lastPos, (transform.position - lastPos), out hit, Vector3.Distance(lastPos, transform.position), LayerMask.NameToLayer("Player Bullet"))) {
            // Makes the particle effects look a lot better with high speed bullets, and also makes sure they don't clip too much
            particleEmitter.transform.position = hit.point;
        }

        particleEmitter.Play();
    }
}
