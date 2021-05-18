using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitChildren : MonoBehaviour
{
    public int maxChildCount = 1;

    void Update()
    {
        if (transform.childCount > maxChildCount)
        {
            for (int i = 0; i < (transform.childCount - maxChildCount); i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
