using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MultiLight : MonoBehaviour
{
    public Color color = new Color(1, 1, 1);
    [Range(0f, 2.0f)] public float intensity = 1f;
    void Update()
    {
         for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Light>().color = color;
            transform.GetChild(i).GetComponent<Light>().intensity = intensity;
        }
    }
}
