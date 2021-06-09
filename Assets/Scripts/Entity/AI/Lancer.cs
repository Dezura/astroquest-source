using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lancer : EnemyAI
{
    Vector3 randomizedFollowOffset;

    float randomizedFollowOffsetRange = 100f;
    float randomizedFollowOffsetChangeRate = 1f;

    public override void Init()
    {
        enemyAvoidanceDistance = 45f;

        StartCoroutine("RandomizeFollowOffset");
    }

    public override void UpdateAI()
    {
        transform.LookAt(target, target.up);

        gunManager.AimGunPoints(g.playerShip.transform.position, transform.up);

        MoveTowards(target.position + randomizedFollowOffset);
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

    private IEnumerator RandomizeFollowOffset()
    {
        yield return new WaitForSeconds(randomizedFollowOffsetChangeRate);
        randomizedFollowOffset.x = Random.Range(-randomizedFollowOffsetRange/2f, randomizedFollowOffsetRange/2f);
        randomizedFollowOffset.y = Random.Range(-randomizedFollowOffsetRange/2f, randomizedFollowOffsetRange/2f);
        randomizedFollowOffset.z = Random.Range(-randomizedFollowOffsetRange/2f, randomizedFollowOffsetRange/2f);

        StartCoroutine("RandomizeFollowOffset");
    }
}