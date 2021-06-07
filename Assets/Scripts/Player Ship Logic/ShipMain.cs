using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMain : BaseAI
{
    public Camera cam;
    public Cinemachine.CinemachineVirtualCamera vCam;
    
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

        gunManager.SetCurrentGun("Blaster");
        gunManager.AimGunPoints(shipLook.aimPoint.position, modelTransform.up, true);
    }

    void Start() {} // Override BaseAI Start()

    public override void UpdateAI()
    {
        CheckMouseClick(Mouse.current);

        shipMovement.UpdateThrusterSpeeds();
    }

    public override void FixedUpdateAI()
    {
        shipLook.UpdateCameraPoints(); // Sets positions of camera points, then the camera automatically updates its position based on the points
        shipLook.UpdateAimPoint();

        shipMovement.HandleMovement();
        shipLook.HandleMainRotation();

        shipLook.ApplyModelRotations();

        gunManager.AimGunPoints(shipLook.aimPoint.position, modelTransform.up);
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
            }
            else entity.rigidBody.AddExplosionForce(contactKnockback * 1000, averagedHitPoint, 1f);
        }

        else entity.rigidBody.AddExplosionForce(contactKnockback * 1000, averagedHitPoint, 1f);
    }

    public override void OnDeath(GameObject hitSource)
    {
        Debug.Log("You were killed by " + hitSource.name);
    }
}
