using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orion : EnemyAI
{
    public override void Init()
    {
        base.Init();

        playerAvoidanceDistance = 20f;
        enemyAvoidanceDistance = 20f;

        mouseAvoidanceDistance = 17500f;
    }

    public override void UpdateAI()
    {
        base.UpdateAI();

        transform.LookAt(target, target.up);
        gunManager.AimGunPoints(g.playerShip.transform.position, transform.up);

        if (currentDetection != "None") gunManager.Shoot();
    }

    public override void FixedUpdateAI()
    {
        base.FixedUpdateAI();

        if (currentDetection != "Close") MoveTowards(target.position);

        if (currentDetection != "None") {AvoidNearbyContactFrom("Enemy"); AvoidMouse();}
        if (currentDetection == "Close") AvoidNearbyContactFrom("Player");
    }
}
