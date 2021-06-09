using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orion : EnemyAI
{
    public override void Init()
    {
        playerAvoidanceDistance = 20f;
        enemyAvoidanceDistance = 20f;

        mouseAvoidanceDistance = 15000f;
    }

    public override void UpdateAI()
    {
        transform.LookAt(target, target.up);

        gunManager.AimGunPoints(g.playerShip.transform.position, transform.up);
    }

    public override void WhileOutOfRange()
    {
        MoveTowards(target.position);
    }

    public override void WhileInAttackRange()
    {
        AvoidMouse();

        MoveTowards(target.position);
        
        gunManager.Shoot();

        AvoidNearbyContactFrom("Enemy");
    }

    public override void WhileInCloseRange()
    {
        AvoidMouse();

        gunManager.Shoot();

        AvoidNearbyContactFrom("Enemy");
        AvoidNearbyContactFrom("Player");
    }

    public override void WhileDead() 
    {
        base.WhileDead();
    }
}
