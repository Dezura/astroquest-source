using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helminth : EnemyAI
{
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

       // TODO: Add aiming and shooting logic 
    }

    public override void OnHit(GameObject hitSource, float damage, float forceApplied = 0)
    {
        
    }

    public override void OnDeath(GameObject hitSource)
    {
        // TODO: Add explosion and random flying scrap
        // TODO: Add screenshake on death
    }

    public override void WhileDead() 
    {
        Destroy(gameObject);
    }
}
