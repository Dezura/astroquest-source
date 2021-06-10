using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liberator : EnemyAI
{
    public override void Init()
    {
        playerAvoidanceDistance = 45f;
        enemyAvoidanceDistance = 45f;
    }

    public override void UpdateAI()
    {
        transform.LookAt(target, target.up);
        gunManager.AimGunPoints(g.playerShip.transform.position, transform.up);

        if (currentDetection != "None") gunManager.Shoot();
    }

    public override void FixedUpdateAI()
    {
        if (currentDetection != "Close") MoveTowards(target.position);

        if (currentDetection != "None") {AvoidNearbyContactFrom("Enemy"); AvoidMouse();}
        if (currentDetection == "Close") AvoidNearbyContactFrom("Player");
    }
}
