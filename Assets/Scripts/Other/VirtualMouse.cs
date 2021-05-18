using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VirtualMouse : Utils
{
    public RectTransform virtualMouseTransform;
    public float mouseSensitivity = 100f;
    public Vector2 virtualPositionClamp;

    // This variable is kinda just a band aid fix, too lazy to figure out a better solution
    bool resettedOnFocus = false;

    [HideInInspector] public Vector2 vMousePosition;
    [HideInInspector] public Vector2 vMouseOffset; // Didn't know what to name this variable

    Mouse mouse;
    [HideInInspector] public Vector2 mouseMoveDelta;
    Vector2 mousePos;
    Vector2 mousePosLast;

    void Update()
    {
        mouse = Mouse.current;
        
        HandleCursorPositions();
        UpdateMouseValues();
        ClampVirtualMousePosition();

        vMousePosition = virtualMouseTransform.position;
        vMouseOffset = new Vector2((vMousePosition.x - Screen.width/2) / virtualPositionClamp.x, (vMousePosition.y - Screen.height/2) / virtualPositionClamp.y);
    }

    // This function is kinda a mess ngl
    public void HandleCursorPositions()
    {
        if (Application.isFocused) {
            Cursor.visible = false;
            if (!PointWithinBoundExtents(mousePos, new Vector2(Screen.width/2f, Screen.height/2f), 350, 350)) {
                ResetCursorPosition();
            }
            virtualMouseTransform.position += (Vector3) mouseMoveDelta * (mouseSensitivity/100f);

            resettedOnFocus = false;
        }
        else {
            if (!resettedOnFocus) {
                mouse.WarpCursorPosition(virtualMouseTransform.position);
                resettedOnFocus = true;
            }
        }
    }

    public void UpdateMouseValues()
    {
        mousePos = mouse.position.ReadValue();
        if (mousePosLast == Vector2.zero && Application.isFocused) {
            mousePosLast = mousePos;
        }
        mouseMoveDelta = mousePos - mousePosLast;
        mousePosLast = mousePos;
    }

    public void ClampVirtualMousePosition()
    {
        float clampedX = Mathf.Clamp(virtualMouseTransform.position.x, Screen.width/2f - virtualPositionClamp.x, Screen.width/2f + virtualPositionClamp.x);
        float clampedY = Mathf.Clamp(virtualMouseTransform.position.y, Screen.height/2f - virtualPositionClamp.y, Screen.height/2f + virtualPositionClamp.y);

        virtualMouseTransform.position = new Vector2(clampedX, clampedY);
    }

    public void ResetCursorPosition()
    {
        mousePos = mouse.position.ReadValue();
        mousePosLast = mousePos;
        mouseMoveDelta = Vector2.zero;
        
        mouse.WarpCursorPosition(new Vector2(Screen.width/2f, Screen.height/2f));
    }
}
