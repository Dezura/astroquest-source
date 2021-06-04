using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helminth : EnemyAI
{
    float playerAvoidanceDistance = 20f;
    float enemyAvoidanceDistance = 20f;
    float mouseAvoidanceDistance = 250f;

    public override void UpdateAI()
    {
        transform.LookAt(target, target.up);
        
        AvoidNearbyContact();
    }

    public override void WhileOutOfRange()
    {
        MoveTowards(target.position);
    }

    public override void WhileInAttackRange()
    {
        AvoidMouse();
    }

    public override void OnHit(GameObject hitSource, float damage, float forceApplied = 0)
    {
        
    }

    public override void OnDeath(GameObject hitSource)
    {
        
    }

    public override void WhileDead() 
    {
        Destroy(gameObject);
    }

    public void AvoidMouse()
    {        
        if (Vector2.Distance(g.mainCamera.WorldToScreenPoint(transform.position), g.virtualMouse.vMousePosition) <= mouseAvoidanceDistance) { // If near mouse
            Vector2 dirFromMouse = (((Vector2) g.mainCamera.WorldToScreenPoint(transform.position)) - g.virtualMouse.vMousePosition).normalized; // Calculate direction from the mouse to the enemy relative to screenspace
            Vector3 moveDir = g.mainCamera.transform.rotation * new Vector3(dirFromMouse.x, dirFromMouse.y, 0); // Convert the direction to a Vector3 in worldspace

            entity.rigidBody.AddForce(moveDir * speed); // Then move using the new moveDir
        }
    }

    public void AvoidNearbyContact()
    {
        foreach (Entity detectedEntity in detectionArea.overlappingEntities) { // First avoid colliding with enemies and keep distance (This also give an illusion of a more complex groupthinking type AI)
            if (!detectedEntity) continue; // If the entity is dead, but it's still in the overlapping list, skip to next iteration
            EnemyAI enemy;
            ShipMain playerShip;

            if (detectedEntity.TryGetComponent<EnemyAI>(out enemy)) {
                if (Vector3.Distance(transform.position, enemy.transform.position) <= enemyAvoidanceDistance) {
                    Vector3 moveDir = (transform.position - enemy.transform.position).normalized;

                    entity.rigidBody.AddForce(moveDir * speed);
                }
            }

            else if (detectedEntity.TryGetComponent<ShipMain>(out playerShip)) {
                if (Vector3.Distance(transform.position, playerShip.transform.position) <= playerAvoidanceDistance) {
                    Vector3 moveDir = (transform.position - playerShip.transform.position).normalized;

                    entity.rigidBody.AddForce(moveDir * speed);
                }
            }
        }
    }
}
