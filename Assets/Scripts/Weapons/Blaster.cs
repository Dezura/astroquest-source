using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : BaseGun
{
    public List<Transform> shootPoints = new List<Transform>();

    int currentShootPoint = 0;

    public override void Init()
    {
        base.Init();

        gunModelPrefab = g.assets.gunModels[tag]["Blaster"];

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
            newModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public override void FixStats()
    {
        base.FixStats();

        firerate *= gunManager.gunPoints.Count;

        autofire = true;
        firerate *= 1.5f;
        damage *= 1.25f;
        projectileSize *= 1.25f;
    }

    public override void OnShoot()
    {
        base.OnShoot();

        // TODO: Add a muzzle flash for more impact on each shot

        if (!canShoot) return;

        SpawnBullet(shootPoints[currentShootPoint].position, shootPoints[currentShootPoint].rotation * Quaternion.Euler(0, 180, 0));

        currentShootPoint += 1;
        if (currentShootPoint >= shootPoints.Count) currentShootPoint = 0;

        StartCoroutine("FirerateRefresh");
    }
}
