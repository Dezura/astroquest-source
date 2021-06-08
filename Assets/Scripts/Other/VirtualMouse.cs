using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VirtualMouse : Utils
{
    public List<RectTransform> virtualMouseTransform; // Making these arrays to store both of the cursor states, as it moves a seperate cursor on right click
    public float mouseSensitivity = 100f;
    public Vector2 virtualPositionClamp;

    [HideInInspector] public bool isAiming = false;

    // This variable is kinda just a band aid fix, too lazy to figure out a better solution
    [HideInInspector] bool resettedOnFocus = false;

    [HideInInspector] public List<Vector2> vMousePosition;
    [HideInInspector] public List<Vector2> vMouseOffset;

    Mouse mouse;
    [HideInInspector] public Vector2 mouseMoveDelta;
    Vector2 mousePos;
    Vector2 mousePosLast;

    void Awake() 
    {
        vMousePosition.Add(Vector2.zero);
        vMousePosition.Add(Vector2.zero);
        vMouseOffset.Add(Vector2.zero);
        vMouseOffset.Add(Vector2.zero);
    }


    void Update()
    {
        mouse = Mouse.current;
        
        isAiming = mouse.rightButton.isPressed;
        
        if (isAiming) UpdateSecondaryMouse();
        else UpdateMainMouse();
        ClampVirtualMousePositions();
        UpdateMouseValues();

        vMousePosition[0] = virtualMouseTransform[0].position;
        vMouseOffset[0] = new Vector2((vMousePosition[0].x - Screen.width/2) / virtualPositionClamp.x, (vMousePosition[0].y - Screen.height/2) / virtualPositionClamp.y);
        vMousePosition[1] = virtualMouseTransform[1].position;
        vMouseOffset[1] = new Vector2((vMousePosition[1].x - Screen.width/2) / virtualPositionClamp.x, (vMousePosition[1].y - Screen.height/2) / virtualPositionClamp.y);
    }

    // This function is kinda a mess ngl
    public void UpdateMainMouse()
    {
        if (Application.isFocused) {
            Cursor.visible = false;
            if (!PointWithinBoundExtents(mousePos, new Vector2(Screen.width/2f, Screen.height/2f), 350, 350)) {
                ResetCursorPosition();
            }
            virtualMouseTransform[0].position += (Vector3) mouseMoveDelta * (mouseSensitivity/100f);
            virtualMouseTransform[1].localPosition = Vector2.zero;

            resettedOnFocus = false;
        }
        else {
            if (!resettedOnFocus) {
                mouse.WarpCursorPosition(virtualMouseTransform[0].position);
                resettedOnFocus = true;
            }
        }
    }

    public void UpdateSecondaryMouse()
    {
        if (Application.isFocused) {
            Cursor.visible = false;

            virtualMouseTransform[1].position += (Vector3) mouseMoveDelta * (mouseSensitivity/100f);

            resettedOnFocus = false;
        }
        else {
            if (!resettedOnFocus) {
                mouse.WarpCursorPosition(virtualMouseTransform[0].position);
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

    public void ClampVirtualMousePositions()
    {
        float clampedX1 = Mathf.Clamp(virtualMouseTransform[0].position.x, Screen.width/2f - virtualPositionClamp.x, Screen.width/2f + virtualPositionClamp.x);
        float clampedY1 = Mathf.Clamp(virtualMouseTransform[0].position.y, Screen.height/2f - virtualPositionClamp.y, Screen.height/2f + virtualPositionClamp.y);

        virtualMouseTransform[0].position = new Vector2(clampedX1, clampedY1);

        float clampedX2 = Mathf.Clamp(virtualMouseTransform[1].position.x, 0, Screen.width);
        float clampedY2 = Mathf.Clamp(virtualMouseTransform[1].position.y, 0, Screen.height);

        virtualMouseTransform[1].position = new Vector2(clampedX2, clampedY2);
    }

    public void ResetCursorPosition()
    {
        mousePos = mouse.position.ReadValue();
        mousePosLast = mousePos;
        mouseMoveDelta = Vector2.zero;
        
        mouse.WarpCursorPosition(new Vector2(Screen.width/2f, Screen.height/2f));
    }
}
