using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helminth : EnemyAI
{
    public override void Init()
    {
        base.Init();

        enemyAvoidanceDistance = 30f;
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

        MoveTowards(target.position);

        if (currentDetection != "None") AvoidNearbyContactFrom("Enemy");
    }
}
