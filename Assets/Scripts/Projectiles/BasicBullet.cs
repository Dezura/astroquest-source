using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : Projectile
{
    public override void FixStats()
    {
        base.FixStats();

        damage *= 2f;
    }

    public override void FixedUpdateProjectile()
    {
        base.FixedUpdateProjectile();
        
        rigidBody.velocity = transform.forward * speed;
    }

    public override void OnHitCollision(Collider collider)
    {
        base.OnHitCollision(collider);

        RaycastHit hit;
        if(Physics.Raycast(lastPos, (transform.position - lastPos), out hit, Vector3.Distance(lastPos, transform.position), g.layerMasks[tag])) {
            // Makes the particle effects look a lot better with high speed bullets, and also makes sure they don't clip too much
            // This is also used to accurately apply knockback to entities
            transform.position = hit.point;
        }

        EntityForwarder entityForwarder;
        if (collider.TryGetComponent<EntityForwarder>(out entityForwarder)) {
            entityForwarder.targetEntity.OnHit(gameObject, damage, knockbackForce);
        }

        OnDestroy();
    }
}
