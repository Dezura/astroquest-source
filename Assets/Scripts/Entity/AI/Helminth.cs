using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helminth : EnemyAI
{
    public override void Init()
    {
        gunManager.SetCurrentGun("Blaster");
        gunManager.AimGunPoints(g.playerShip.transform.position, transform.up, true);

        playerAvoidanceDistance = 30f;
        enemyAvoidanceDistance = 30f;
    }

    public override void UpdateAI()
    {
        transform.LookAt(target, target.up);

        gunManager.AimGunPoints(g.playerShip.transform.position, transform.up);

        MoveTowards(target.position);
    }

    public override void WhileInAttackRange()
    {
        gunManager.Shoot();

        AvoidNearbyContactFrom("Enemy");
    }

    public override void WhileInCloseRange()
    {
        gunManager.Shoot();

        AvoidNearbyContactFrom("Enemy");
    }

    public override void WhileDead() 
    {
        base.WhileDead();
    }
}
