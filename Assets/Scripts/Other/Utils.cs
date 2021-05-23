using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    [HideInInspector] public Globals g;

    void Awake()
    {
        GetGlobals();
    }

    // Call this function if you override Awake()
    public void GetGlobals()
    {
        g = transform.root.Find("Globals and Other").GetComponent<Globals>();
    }

    // This assumes point1 is the top left corner of the bounds, and point2 bottom right
    public bool PointWithinBoundPoints(Vector2 point, Vector2 boundsPoint1, Vector2 boundsPoint2)
    {
        bool condition1 = (point.x > boundsPoint1.x && point.x < boundsPoint2.x);
        bool condition2 = (point.y < boundsPoint1.y && point.y > boundsPoint2.y);
        if (condition1 && condition2) {
            return true;
        }

        return false;
    }

    public bool PointWithinBoundExtents(Vector2 point, Vector2 boundsPos, float extentsX, float extentsY)
    {
        bool condition1 = (point.x > (boundsPos.x - extentsX) && point.x < (boundsPos.x + extentsX));
        bool condition2 = (point.y > (boundsPos.y - extentsY) && point.y < (boundsPos.y + extentsY));
        if (condition1 && condition2) {
            return true;
        }

        return false;
    }

}
