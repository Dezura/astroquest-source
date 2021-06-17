using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class BaseGun : Utils
{
    public LayerMask bulletMask;

    public GunManager gunManager;

    public GameObject bulletPrefab;
    public GameObject gunModelPrefab;

    public List<GameObject> gunModels = new List<GameObject>();

    [Header("Gun Stats (Some stats may not be applicable to every gun type)")]
    public bool autofire = false;
    public float firerate = 40f;
    public float multishot = 1f;
    public float accuracy = 0.1f;

    [Header("Bullet Stats (Some stats may not be applicable to every bullet type)")]
    public float damage = 75f;
    public float projectileSize = 1.3f;
    public float projectileSpeed = 100f;
    public float projectileLifespan = 1f;
    public float projectileKnockback = 1f;

    [HideInInspector] public bool canShoot = true;
    
    void Awake() {} // This overrides the Awake() in Utils

    void Start() {Init();}



    // Override these functions in ai scripts as needed

    public virtual void Init() 
    {
        GetGlobals();
        
        FixStats();
    }

    public virtual void OnShoot() {}

    public virtual void FixStats() {} // Use this to adjust certain stats (eg, less damage per projectile on shotguns)



    // Extra helper/other type functions

    public IEnumerator<float> FirerateRefresh()
    {
        canShoot = false;
        yield return Timing.WaitForSeconds(10f/firerate);
        canShoot = true;
    }

    public void SpawnBullet(Vector3 position, Quaternion rotation)
    {
        var projectile = Instantiate(bulletPrefab, position, rotation).GetComponent<Projectile>();

        projectile.transform.SetParent(g.projectileSpawn);
        
        // Made a temp variable as I can't directly use Scale() on the transform, for some reason
        Vector3 newScale = projectile.transform.localScale; newScale.Scale(Vector3.one * projectileSize); 
        projectile.transform.localScale = newScale;
        
        projectile.damage = damage;
        projectile.speed = projectileSpeed;
        projectile.lifespan = projectileLifespan;
        projectile.sizeMultiplier = projectileSize;
        projectile.knockbackForce = projectileKnockback;
    }
}
