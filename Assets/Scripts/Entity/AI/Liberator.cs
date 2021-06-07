using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liberator : EnemyAI
{
    public override void Init()
    {
        gunManager.SetCurrentGun("Blaster");
        gunManager.AimGunPoints(g.playerShip.transform.position, transform.up, true);

        playerAvoidanceDistance = 45f;
        enemyAvoidanceDistance = 45f;
    }

    public override void UpdateAI()
    {
        transform.LookAt(target, target.up);
    }

    public override void WhileOutOfRange()
    {
        MoveTowards(target.position);
    }

    public override void WhileInAttackRange()
    {
        MoveTowards(target.position);

        gunManager.AimGunPoints(g.playerShip.transform.position, transform.up);
        gunManager.Shoot();

        AvoidNearbyContactFrom("Enemy");
    }

    public override void WhileInCloseRange()
    {
        AvoidMouse();

        gunManager.AimGunPoints(g.playerShip.transform.position, transform.up);
        gunManager.Shoot();

        AvoidNearbyContactFrom("Player");
    }
    
    public override void WhileDead() 
    {
        Destroy(gameObject);
    }
}
