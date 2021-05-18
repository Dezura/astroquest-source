using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLook : Utils
{
    private ShipMain main;

    public Transform aimPoint;
    public Transform[] cameraPoints = new Transform[3];

    public Vector2 lookRotationAmount = Vector2.one * 100;

    public Vector2 cameraAdaptiveLookAmount = Vector2.one * 10;
    public float cameraAdaptiveFollowAmount = 0.5f;

    public float lookLerp = 0.35f;

    public float torqueLerp = 0.35f;

    public float aimTorqueAmount = 140f;

    public float aimMaxDistance = 500f;
    public float aimZCutoff = 13.75f;

    public Transform[] aimGunTransforms;
    public float aimGunSpeed = 5f;

    [HideInInspector] public Dictionary<string, Vector3> modelTorques = new Dictionary<string, Vector3>();

    Vector3 camPoint1StartPos;

    VirtualMouse vm;

    Vector2[] lerpMousePos= new Vector2[2];

    void Start()
    {
        main = transform.GetComponentInParent<ShipMain>();
        vm = g.virtualMouse;

        modelTorques.Add("Look", Vector3.zero); // Doing this instead of adding directly so it can be consistent

        camPoint1StartPos = cameraPoints[1].localPosition;
    }

    public void HandleMainRotation()
    {
        transform.Rotate(new Vector3(-(lookRotationAmount.y/10f * vm.vMouseOffset.y) * Time.fixedDeltaTime * 150f, lookRotationAmount.x/10f * vm.vMouseOffset.x * Time.fixedDeltaTime * 150f, 0), Space.Self);
    }

    public void ApplyModelRotations()
    {
        lerpMousePos[0] = Vector2.Lerp(lerpMousePos[0], vm.vMousePosition, lookLerp * Time.fixedDeltaTime * 100);
        Ray screenRay = main.cam.ScreenPointToRay(lerpMousePos[0]);

        main.modelTransform.LookAt(screenRay.GetPoint(aimMaxDistance), transform.up);

        AddModelTorque("Look", new Vector3(0, 0, -vm.vMouseOffset.x * aimTorqueAmount));

        // Does the thing where the ship model rotates left or right, depending on forces
        foreach (Vector3 torque in modelTorques.Values)
        {
            main.modelTransform.rotation *= Quaternion.Euler(torque.x, torque.y, torque.z);
        }
    }

    public void UpdateCameraPoints()
    {
        // Point 1
        cameraPoints[1].localPosition = camPoint1StartPos + new Vector3(vm.vMouseOffset.x * cameraAdaptiveLookAmount.x, vm.vMouseOffset.y * cameraAdaptiveLookAmount.y, 0);

        // Point 2
        cameraPoints[2].position = Vector3.Lerp(cameraPoints[0].position, cameraPoints[1].position, cameraAdaptiveFollowAmount);

    }

    public void UpdateAimPoint()
    {
        Ray screenRay = main.cam.ScreenPointToRay(g.virtualMouse.vMousePosition);
        RaycastHit hit;

        if (Physics.Raycast(screenRay.origin, screenRay.direction, out hit, aimMaxDistance, LayerMask.NameToLayer("Player Bullet") ) && ((Quaternion.Inverse(main.cam.transform.rotation) * (hit.point - main.cam.transform.position)).z > aimZCutoff)) {
            aimPoint.position = hit.point;
        }
        else {
            aimPoint.position = screenRay.GetPoint(aimMaxDistance/8f);
        }
    }

    public void AddModelTorque(string torqueSource, Vector3 torqueAmount)
    {
        modelTorques[torqueSource] = Vector3.Lerp(modelTorques[torqueSource], torqueAmount, torqueLerp);
    }

    public void HandleAimGunsRotations()
    {
        foreach (Transform gunTransform in aimGunTransforms)
        {
            gunTransform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(-gunTransform.forward, aimPoint.position - gunTransform.position, aimGunSpeed * Time.fixedDeltaTime, 0.0f), main.modelTransform.up) * Quaternion.Euler(0, 180, 0);
        }
    }
}