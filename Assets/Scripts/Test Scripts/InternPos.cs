using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class InternPos : MonoBehaviour
{
    public Transform rootTransform;
    public Transform targetTransform;

    [Range(0f, 1.0f)]public float interpAmount = 0.5f;

    void Update()
    {
        transform.position = Vector3.Lerp(rootTransform.position, targetTransform.position, interpAmount);
    }
}
