using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Chaingun : BaseGun
{
    public List<Transform> shootPoints = new List<Transform>();

    public override void Init()
    {
        base.Init();

        gunModelPrefab = g.assets.gunModels[tag]["Chaingun"];

        bulletMask = g.layerMasks[tag + " Projectile"];

        foreach (Transform gunPoint in gunManager.gunPoints)
        {
            GameObject newModel = Instantiate(gunModelPrefab);
            newModel.transform.parent = gunPoint;
            newModel.tag = tag;

            gunModels.Add(newModel);
            newModel.transform.GetChild(0).rotation = Quaternion.Euler(180, 0, 0);
            shootPoints.Add(newModel.transform.GetChild(0));

            newModel.transform.localScale = Vector3.Scale(newModel.transform.localScale, gunPoint.localScale);
            newModel.transform.localPosition = Vector3.zero;
            newModel.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public override void FixStats()
    {
        base.FixStats();

        autofire = true;
        firerate *= 5f;
        damage /= 3f;
        projectileSize /= 1.25f;
        projectileSpeed *= 1.25f;
    }

    public override void OnShoot()
    {
        base.OnShoot();

        foreach (GameObject model in gunModels)
        {
            model.transform.GetChild(1).Rotate(Vector3.forward * 100 * Time.fixedDeltaTime);
        }

        if (!canShoot) return;

        foreach (Transform shootPoint in shootPoints)
        {
            Vector3 randomPosOffset = new Vector3();
            Vector3 randomRot = new Vector3();

            randomPosOffset.x = Random.Range(shootPoint.GetChild(0).localPosition.x * shootPoint.GetChild(0).lossyScale.x * 1.5f, shootPoint.GetChild(1).localPosition.x * shootPoint.GetChild(1).lossyScale.x * 1.5f);
            randomPosOffset.y = Random.Range(shootPoint.GetChild(0).localPosition.y * shootPoint.GetChild(0).lossyScale.y * 1.5f, shootPoint.GetChild(1).localPosition.y * shootPoint.GetChild(1).lossyScale.y * 1.5f);
            randomPosOffset = shootPoint.rotation * randomPosOffset;

            randomRot.x = Random.Range(-5, 5);
            randomRot.y = Random.Range(-5, 5);

            SpawnBullet(shootPoint.position + randomPosOffset, shootPoint.rotation * Quaternion.Euler(randomRot.x, randomRot.y, randomRot.z) * Quaternion.Euler(0, 180, 0));
        }
        

        Timing.RunCoroutine(FirerateRefresh().CancelWith(gameObject));
    }
}
