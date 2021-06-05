using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMain : BaseAI
{
    public Camera cam;
    public Cinemachine.CinemachineVirtualCamera vCam;
    
    public Transform modelTransform;
    
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

    public override void OnDeath(GameObject hitSource)
    {
        Debug.Log("You were killed by " + hitSource.name);
    }
}
