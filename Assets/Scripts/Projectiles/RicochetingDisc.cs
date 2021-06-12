using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetingDisc : Projectile
{
    public override void FixStats()
    {
        base.FixStats();

        transform.localScale /= 1.5f;
        lifespan *= 2f;
        speed *= 1.2f;
        damage /= 1.25f;
    }

    public override void FixedUpdateProjectile()
    {
        base.FixedUpdateProjectile();

        model.transform.localRotation *= Quaternion.Euler(Vector3.forward * 2500 * Time.fixedDeltaTime);
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

        if (tag == "Enemy Projectile" || !entityForwarder) {OnDestroy(); return;}

        if (collider.TryGetComponent<EntityForwarder>(out entityForwarder)) {
            EnemyAI target = GetClosestEnemy(entityForwarder.targetEntity.transform);
            if (target) transform.LookAt(target.transform);
        }
    }
}
