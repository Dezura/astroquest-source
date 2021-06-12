using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBullet : Projectile
{
    public override void FixedUpdateProjectile()
    {
        base.FixedUpdateProjectile();

        rigidBody.velocity = transform.forward * speed;
    }

    public override void OnHitCollision(Collider collider)
    {
        base.OnHitCollision(collider);
        destroyExplosion.Play();

        Vector3 hitPoint = transform.position;

        RaycastHit hit;
        if(Physics.Raycast(lastPos, (transform.position - lastPos), out hit, Vector3.Distance(lastPos, transform.position), g.layerMasks[tag])) {
            hitPoint = hit.point;
        }

        EntityForwarder entityForwarder;
        if (collider.TryGetComponent<EntityForwarder>(out entityForwarder)) {
            entityForwarder.targetEntity.OnHit(gameObject, damage, knockbackForce, hitPoint);
        }
    }
}
