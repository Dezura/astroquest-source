using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMovement : Utils
{
    private ShipMain main;

    public float moveSpeed = 100f;
    public float rollSpeed = 50f;

    public float rollAmount = 20f;

    public Dictionary<string, Vector2> moveModelOffset = new Dictionary<string, Vector2>
    {
        {"max", new Vector2(40f, 40f)},
        {"lerpSpeed", new Vector2(7.5f, 7.5f)},
        {"current", Vector2.zero}
    };

    public Dictionary<string, float> moveModelTorque = new Dictionary<string, float>
    {
        {"max", 40f},
        {"lerpSpeed", 7.5f},
        {"current", 0f}
    };

    public GameObject thrusterRoot;
    public Dictionary<string, float> thrustersSpeed = new Dictionary<string, float>
    {
        {"max", 100f},
        {"lerpSpeed", 7.5f},
        {"current", 0f}
    };

    // Yes, I'm using a 4D vector lol
    // The W axis here will be used to store the roll rotation values
    // Is it unnecessary? Yeah for the most part, but atleast now I can say "Oh I've written code with 4D vectors!" or something
    Vector4 inputVector = Vector4.zero;

    public float cameraBoostZoom = 60f;
    public float speedBoostAmount = 1.5f;
    [HideInInspector] public bool boostInput = false;
    [HideInInspector] public bool isBoosting = false;

    Vector3 clampedMoveDir;

    void Start()
    {
        main = transform.GetComponentInParent<ShipMain>();

        main.shipLook.modelTorques.Add("Movement", Vector3.zero);
    }

    // This function is called everytime a movement related input is called
    public void SetMoveInput(InputAction.CallbackContext inputContext) 
    {
        switch (inputContext.action.name)
        {
            case "MoveXZ": {
                Vector2 input = inputContext.ReadValue<Vector2>();

                inputVector.x = input.x;
                inputVector.z = input.y;
                break;
            }

            case "MoveY": {
                float input = inputContext.ReadValue<float>();

                inputVector.y = input;
                break;
            }

            case "MoveW": {
                float input = inputContext.ReadValue<float>();

                inputVector.w = input;
                break;
            }

            default: {
                Debug.LogError("Couldn't match action to case!");
                break;
            }
        }

        clampedMoveDir = Vector3.ClampMagnitude(inputVector, 1);
    }

    public void SetBoostInput(InputAction.CallbackContext inputContext)
    {
        float input = inputContext.ReadValue<float>();

        if (input == 1) boostInput = true;
        else if (input == 0) boostInput = false;
    }

    public void HandleMovement()
    {
        isBoosting = (boostInput && !g.virtualMouse.isAiming && (inputVector.z == 1 && inputVector.x == 0 && inputVector.y == 0));

        // Applies basic movement force for x and y, that force then rotated by it's own rotation
        main.entity.rigidBody.AddForce(transform.rotation * (new Vector3(clampedMoveDir.x, clampedMoveDir.y, 0) * moveSpeed * 12f));

        // Applies special force to z
        // If moving forward, it rotates velocity by the model rotation instead of it's own rotation
        if (clampedMoveDir.z > 0) {
            if (isBoosting) { // Handles boosting
                main.playerCamera.ApplyZoom(cameraBoostZoom);
                main.entity.rigidBody.AddForce(main.modelTransform.rotation * (new Vector3(0, 0, clampedMoveDir.z) * moveSpeed * 17f * speedBoostAmount));
            }
            else main.entity.rigidBody.AddForce(main.modelTransform.rotation * (new Vector3(0, 0, clampedMoveDir.z) * moveSpeed * 17f));
        }
        // Else if moving backwards, it rotates by it's own rotation
        else {
            main.entity.rigidBody.AddForce(transform.rotation * (new Vector3(0, 0, clampedMoveDir.z) * moveSpeed * 12f));
        }

        // Handles rolling and stuff with the w axis of inputVector
        main.entity.rigidBody.AddTorque(main.modelTransform.rotation * new Vector3(0, 0, -inputVector.w * rollAmount * 12f));

        // Applies rotation force to model based on x movement
        moveModelTorque["current"] = Mathf.Lerp(moveModelTorque["current"], -Mathf.Clamp(clampedMoveDir.x + inputVector.w, -1, 1) * moveModelTorque["max"], moveModelTorque["lerpSpeed"] * Time.fixedDeltaTime);
        main.shipLook.AddModelTorque("Movement", new Vector3(0, 0, moveModelTorque["current"]));
    }

    public void UpdateThrusterSpeeds()
    {
        float thrusterMaxMulti = clampedMoveDir.z;
        if (isBoosting) thrusterMaxMulti *= 1.6f;
        if (!g.virtualMouse.isAiming) thrusterMaxMulti += Vector2.ClampMagnitude(g.virtualMouse.vMouseOffset[0], 1).magnitude/1.25f;

        if (thrusterMaxMulti <= 0.4f) thrusterMaxMulti = 0;

        thrustersSpeed["current"] = Mathf.Lerp(thrustersSpeed["current"], thrustersSpeed["max"] * thrusterMaxMulti, thrustersSpeed["lerpSpeed"]);
        foreach (ParticleSystem thruster in thrusterRoot.GetComponentsInChildren<ParticleSystem>())
        {
            // I have to store these objects first before I can set variables on them, not sure why
            ParticleSystem.EmissionModule particleEmission = thruster.emission;
            ParticleSystem.MainModule particleMain = thruster.main;
            
            particleEmission.rateOverTime = thrustersSpeed["current"];
            particleMain.startLifetime = thrustersSpeed["current"]/300f;
        }
    }
}
