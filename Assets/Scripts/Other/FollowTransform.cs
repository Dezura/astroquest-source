using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FollowTransform : MonoBehaviour
{
    [Header("Target Transform")]
    public Transform targetTransform;
    public bool followCurrentCamera = false;

    [Header("Following Params")]
    public bool followPosition = false;
    public bool followRotation = false;
    public bool followScale = false;

    [Header("Offsets")]
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public Vector3 scaleOffset;

    void Update()
    {
        try
        {
        if (followCurrentCamera) {if (Camera.current != null) {targetTransform = Camera.current.transform;}}

        if (followPosition) {transform.position = targetTransform.position + positionOffset;}
        if (followRotation) {transform.rotation = Quaternion.Euler(targetTransform.eulerAngles) * Quaternion.Euler(rotationOffset);}
        if (followScale) {transform.localScale = targetTransform.lossyScale + scaleOffset;}
        }
        
        catch (System.Exception) {}
        
    }
}
