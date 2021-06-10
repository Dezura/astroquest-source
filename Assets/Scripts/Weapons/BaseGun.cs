using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : Utils
{
    public LayerMask bulletMask;

    public GunManager gunManager;

    public GameObject bulletPrefab;
    public GameObject gunModelPrefab;

    public List<GameObject> gunModels;

    [Header("Gun Stats (Some stats may not be applicable to every gun type)")]
    public bool autofire = false;
    public float firerate = 40f;
    public float multishot = 1f;
    public float accuracy = 0.1f;

    [Header("Bullet Stats (Some stats may not be applicable to every bullet type)")]
    public float damage = 100f;
    public float projectileSize = 1.3f;
    public float projectileSpeed = 120f;
    public float projectileLifespan = 1f;
    public float projectileKnockback = 1f;

    [HideInInspector] public bool canShoot = true;
    
    void Awake() {} // This overrides the Awake() in Utils

    void Start()
    {
        GetGlobals();

        Init();
        
        FixStats();
    }

    private IEnumerator FirerateRefresh()
    {
        canShoot = false;
        yield return new WaitForSeconds(10f/firerate);
        canShoot = true;
    }

    public void SpawnBullet(Vector3 position, Quaternion rotation)
    {
        // TODO: Rework bullets with a base system
        var projectile = Instantiate(bulletPrefab, position, rotation).GetComponent<BasicBullet>();

        projectile.transform.SetParent(g.projectileSpawns);
        
        // Made a temp variable as I can't directly use Scale() on the transform, for some reason
        Vector3 newScale = projectile.transform.localScale; newScale.Scale(Vector3.one * projectileSize); 
        projectile.transform.localScale = newScale;

        projectile.transform.localPosition -= projectile.transform.rotation * new Vector3(0, projectile.GetComponent<CapsuleCollider>().height * projectile.transform.localScale.y / 2f, 0);
        
        projectile.damage = damage;
        projectile.speed = projectileSpeed;
        projectile.lifespan = projectileLifespan;
        projectile.sizeMultiplier = projectileSize;
        projectile.knockbackForce = projectileKnockback;
    }

    // Override these functions in ai scripts as needed

    public virtual void Init() {}

    public virtual void OnShoot() {}

    public virtual void FixStats() {} // Use this to adjust certain stats (eg, less damage per projectile on shotguns)
}
