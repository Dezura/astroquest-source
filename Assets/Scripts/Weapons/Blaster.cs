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
        bulletPrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Projectiles/Basic Bullet.prefab", typeof(GameObject));
        gunModelPrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Models/Weapons/Player/Blaster.fbx", typeof(GameObject));

        foreach (Transform point in gunManager.gunPoints)
        {
            GameObject newModel = Instantiate(gunModelPrefab);
            newModel.transform.parent = point;

            shootPoints.Add(point);
            
            newModel.transform.localPosition = Vector3.zero;
            newModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public override void OnShoot()
    {
        SpawnBullet(shootPoints[currentShootPoint].position, shootPoints[currentShootPoint].rotation * Quaternion.Inverse(Quaternion.Euler(-90, 0, 0)));

        currentShootPoint += 1;
        if (currentShootPoint >= shootPoints.Count) currentShootPoint = 0;
    }
}
