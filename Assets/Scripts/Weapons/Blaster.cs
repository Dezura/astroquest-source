using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Blaster : BaseGun
{
    public List<Transform> shootPoints = new List<Transform>();

    int currentShootPoint = 0;

    public override void Init()
    {
        bulletPrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Projectiles/" + gameObject.tag + "/Basic Bullet.prefab", typeof(GameObject));
        gunModelPrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Models/Weapons/" + gameObject.tag + "/Blaster.fbx", typeof(GameObject));

        bulletMask = g.layerMasks[gameObject.tag + " Bullet"];

        foreach (Transform point in gunManager.gunPoints)
        {
            GameObject newModel = Instantiate(gunModelPrefab);
            newModel.transform.parent = point;
            newModel.tag = gameObject.tag;

            shootPoints.Add(point);
            
            newModel.transform.localScale = Vector3.Scale(newModel.transform.localScale, point.localScale);
            newModel.transform.localPosition = Vector3.zero;
            newModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public override void OnShoot()
    {
        // TODO: Add a muzzle flash for more impact on each shot

        if (!canShoot) return;

        SpawnBullet(shootPoints[currentShootPoint].position, shootPoints[currentShootPoint].rotation * Quaternion.Inverse(Quaternion.Euler(-90, 0, 0)));

        currentShootPoint += 1;
        if (currentShootPoint >= shootPoints.Count) currentShootPoint = 0;

        canShoot = false;
        StartCoroutine("FirerateRefresh");
    }

    public override void FixStats()
    {
        firerate *= 1.5f;
        damage *= 1.25f;
        projectileSize += 1f;
    }
}
