using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMain : Utils
{
    public Camera cam;
    public Cinemachine.CinemachineVirtualCamera vCam;
    
    public Transform modelTransform;
    
    public CustomizableGun[] currentGuns;

    [HideInInspector] public PlayerInput controls;
    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public ShipMovement shipMovement;
    [HideInInspector] public ShipLook shipLook;

    void Awake() 
    {
        GetGlobals();
        controls = GetComponent<PlayerInput>();

        rigidBody = GetComponent<Rigidbody>();
        shipMovement = GetComponent<ShipMovement>();
        shipLook = GetComponent<ShipLook>();
    }

    void Update()
    {
        foreach (CustomizableGun gun in currentGuns)
        {
            gun.CheckMouseClick(Mouse.current);
        }
    }

    void FixedUpdate()
    {
        shipLook.UpdateCameraPoints(); // Sets positions of camera points, then the camera automatically updates its position based on the points
        shipLook.UpdateAimPoint();

        shipMovement.HandleMovement();
        shipLook.HandleMainRotation();

        shipLook.ApplyModelRotations();

        shipLook.HandleAimGunsRotations();
    }
}
