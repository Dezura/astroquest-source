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

    // Yes, I'm using a 4D vector lol
    // The W axis here will be used to store the roll rotation values
    // Is it unnecessary? Yeah for the most part, but atleast now I can say "Oh I've written code with 4D vectors!" or something
    Vector4 inputVector = Vector4.zero;

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
        
        // TODO: Normalize x y and z here
    }

    public void HandleMovement()
    {
        // Applies basic movement force for x and y, that force then rotated by it's own rotation
        main.rigidBody.AddForce(transform.rotation * (new Vector3(inputVector.x, inputVector.y, 0) * moveSpeed * 100f));

        // Applies special force to z
        // If moving forward, it rotates velocity by the model rotation instead of it's own rotation
        if (inputVector.z > 0) {
            main.rigidBody.AddForce(main.modelTransform.rotation * (new Vector3(0, 0, inputVector.z) * moveSpeed * 150f));
        }
        // Else if moving backwards, it rotates by it's own rotation
        else {
            main.rigidBody.AddForce(transform.rotation * (new Vector3(0, 0, inputVector.z) * moveSpeed * 100f));
        }

        // Handles rolling and stuff with the w axis of inputVector
        main.rigidBody.AddTorque(main.modelTransform.rotation * new Vector3(0, 0, -inputVector.w * rollAmount * 10f));

        // Applies rotation force to model based on x movement
        moveModelTorque["current"] = Mathf.Lerp(moveModelTorque["current"], -Mathf.Clamp(inputVector.x + inputVector.w, -1, 1) * moveModelTorque["max"], moveModelTorque["lerpSpeed"] * Time.fixedDeltaTime);
        main.shipLook.AddModelTorque("Movement", new Vector3(0, 0, moveModelTorque["current"]));
    }
}
