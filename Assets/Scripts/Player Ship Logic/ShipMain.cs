using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMain : BaseAI
{
    public Camera cam;
    public Cinemachine.CinemachineVirtualCamera vCam;
    
    public Transform modelTransform;
    
    public CustomizableGun[] currentGuns;

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
    }

    void Start() {} // Override BaseAI Start()

    public override void UpdateAI()
    {
        foreach (CustomizableGun gun in currentGuns)
        {
            gun.CheckMouseClick(Mouse.current);
        }

        shipMovement.UpdateThrusterSpeeds();
    }

    public override void FixedUpdateAI()
    {
        shipLook.UpdateCameraPoints(); // Sets positions of camera points, then the camera automatically updates its position based on the points
        shipLook.UpdateAimPoint();

        shipMovement.HandleMovement();
        shipLook.HandleMainRotation();

        shipLook.ApplyModelRotations();

        shipLook.HandleAimGunsRotations();
    }

    public override void OnDeath(GameObject hitSource)
    {
        Debug.Log("You were killed by " + hitSource.name);
    }
}
