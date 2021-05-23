using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Originally I wanted to have multiple classes/scripts for each gun type, 
// but because of conflicting design choices and also mainly C# limitations, I decided to do one main amalgamation of a customizable gun class
public class CustomizableGun : Utils
{
    public enum gunEnum {Blaster, Shotgun, Laser, Missile};
    
    public gunEnum gunType;
    
    public Transform[] shootPoints = new Transform[1];

    // If you're wondering how these variables show up the way they do in the inspector, check out CustomizableGunEditor.cs
    public bool autofire = true;
    public GameObject projectileType;
    public float damage = 40f;
    public float firerate = 20f;
    public float projectileSize = 1f;
    public float multishot = 1f;
    public float projectileSpeed = 60f;
    public float projectileLifespan = 1f;

    public bool canShoot = true;
    
    private IEnumerator FirerateRefresh()
    {
        yield return new WaitForSeconds(10f/firerate);
        canShoot = true;
    }

    public void CheckMouseClick(Mouse mouse)
    {
        if ((autofire && mouse.leftButton.isPressed) || (!autofire && mouse.leftButton.wasPressedThisFrame)) {
            Shoot();
        }
    }

    public void Shoot(bool force = false)
    {
        if (!canShoot && !force) return;

        foreach (Transform bulletSpawn in shootPoints)
        {
            var projectile = Instantiate(projectileType, bulletSpawn.position, bulletSpawn.rotation).GetComponent<BasicBullet>();

            projectile.transform.SetParent(g.projectileSpawns);

            // Made a temp variable as I can't directly use Scale() on the transform, for some reason
            Vector3 newScale = projectile.transform.localScale; newScale.Scale(Vector3.one * projectileSize); 
            projectile.transform.localScale = newScale;

            projectile.transform.localPosition -= projectile.transform.rotation * new Vector3(0, projectile.GetComponent<CapsuleCollider>().height * projectile.transform.localScale.y / 2f, 0);
            
            projectile.damage = damage;
            projectile.speed = projectileSpeed;
            projectile.lifespan = projectileLifespan;
            projectile.sizeMultiplier = projectileSize;
        }
        canShoot = false;
        StartCoroutine("FirerateRefresh");
    }
}

