using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helminth : EnemyAI
{
    public override void Init()
    {
        gunManager.SetCurrentGun("Blaster");
        gunManager.AimGunPoints(g.playerShip.transform.position, transform.up, true);
    }

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
        gunManager.AimGunPoints(g.playerShip.transform.position, transform.up);
        gunManager.Shoot();
    }

    public override void WhileInCloseRange()
    {
        AvoidMouse();
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
