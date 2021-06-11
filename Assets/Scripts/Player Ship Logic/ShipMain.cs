using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMain : BaseAI
{
    public string startingGun;
    public string startingProjectile;

    public PlayerCamera playerCamera;

    public Transform modelTransform;

    public float contactKnockback = 40;
    
    [HideInInspector] public GunManager gunManager;
    [HideInInspector] public PlayerInput controls;
    [HideInInspector] public ShipMovement shipMovement;
    [HideInInspector] public ShipLook shipLook;

    void Awake() 
    {
        GetGlobals();
        entity = GetComponent<Entity>();
        controls = GetComponent<PlayerInput>();

        shipMovement = GetComponent<ShipMovement>();
        shipLook = GetComponent<ShipLook>();

        gunManager = GetComponentInChildren<GunManager>();

        gunManager.SetCurrentGun(startingGun, startingProjectile);
        gunManager.AimGunPoints(shipLook.aimPoint.position, modelTransform.up, true);

        gunManager.currentGun.damage *= 1.5f;
        gunManager.currentGun.projectileSize *= 1.25f;
    }

    void Start() {} // Override BaseAI Start()

    public override void UpdateAI()
    {
        base.UpdateAI();

        CheckMouseClick(Mouse.current);

        shipMovement.UpdateThrusterSpeeds();
    }

    public override void FixedUpdateAI()
    {
        base.FixedUpdateAI();

        shipLook.UpdateCameraPoints(); // Sets positions of camera points, then the camera automatically updates its position based on the points
        shipLook.UpdateMovePoint();
        shipLook.UpdateAimPoint();

        shipMovement.HandleMovement();
        shipLook.HandleMainRotation();

        shipLook.ApplyModelRotations();

        gunManager.AimGunPoints(shipLook.aimPoint.position, modelTransform.up);
    }

    public override void AfterHit(GameObject hitSource, float damage, float forceApplied = 0, Vector3? hitPosition = null)
    {
        base.AfterHit(hitSource, damage, forceApplied, hitPosition);
        
        playerCamera.ApplyScreenShake(0.5f);
    }

    public override void OnDeath(GameObject hitSource)
    {
        base.OnDeath(hitSource);

        Debug.Log("You were killed by " + hitSource.name);
    }

    public void CheckMouseClick(Mouse mouse)
    {
        if ((gunManager.currentGun.autofire && mouse.leftButton.isPressed) || (!gunManager.currentGun.autofire && mouse.leftButton.wasPressedThisFrame)) {
            gunManager.Shoot();
        }
    }

    void OnCollisionEnter(Collision other) 
    {
        // Following code will be used to apply knockback to the player when it hits something, and have it take damage if it hits an enemy.

        Vector3 averagedHitPoint = Vector3.zero;
    
        for (var i = 0; i < other.contactCount; i++)
        {
            averagedHitPoint += other.GetContact(i).point;
        }

        averagedHitPoint /= other.contactCount;

        EntityForwarder hitEntityForwarder;
        if (other.collider.TryGetComponent<EntityForwarder>(out hitEntityForwarder)) {
            EnemyAI enemyAI;
            if (hitEntityForwarder.targetEntity.TryGetComponent<EnemyAI>(out enemyAI)) {
                entity.OnHit(enemyAI.gameObject, enemyAI.contactDamage, 2000, averagedHitPoint); // Take extra knockback if it's from an enemy, and take damage
                playerCamera.ApplyScreenShake(12.5f);
            }
            else {
                entity.rigidBody.AddExplosionForce(contactKnockback * 1000, averagedHitPoint, 1f);
                playerCamera.ApplyScreenShake(10f);
            }
        }

        else {
            entity.rigidBody.AddExplosionForce(contactKnockback * 1000, averagedHitPoint, 1f);
            playerCamera.ApplyScreenShake(10f);
        }
    }
}
