using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : Utils
{
    public BaseGun currentGun;

    public List<Transform> gunPoints;
    public float gunAimSpeed = 5f;

    public void SetCurrentGun(string gunName)
    {
        switch (gunName)
        {
            case "Blaster":
                if (currentGun) RemoveCurrentGun();

                currentGun = transform.GetChild(0).gameObject.AddComponent<Blaster>();
                currentGun.gunManager = this;
                currentGun.Init();

                break;
            
            case "Chaingun": // TODO: Implement these gun types
                break;
            
            case "Shotgun":
                break;

            default:
                break;
        }
        
    }

    public void Shoot()
    {
        currentGun.OnShoot();
    }

    public void AimGunPoints(Vector3 aimPoint, Vector3 worldUp, bool instant = false)
    {
        foreach (Transform point in gunPoints)
        {
            if (instant) {
                point.LookAt(aimPoint, worldUp);
                point.rotation *= Quaternion.Euler(0, 180, 0);
                continue;
            }
            point.rotation = Quaternion.LookRotation(Vector3.RotateTowards(-point.forward, aimPoint - point.position, gunAimSpeed * Time.fixedDeltaTime, 0.0f), worldUp) * Quaternion.Euler(0, 180, 0);
        }
    }

    public void RemoveCurrentGun()
    {
        foreach (Transform point in gunPoints)
        {
            Destroy(point.GetChild(0)); // Destroy each existing gun model
        }

        Destroy(currentGun);
    }
}
