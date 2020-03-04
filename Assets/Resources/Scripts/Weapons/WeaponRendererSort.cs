using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRendererSort : PositionRendererSort
{
    public float minRotateBoundary, maxRotateBoundary;

    override protected void LateUpdate()
    {
        if (transform.localEulerAngles.z < minRotateBoundary || transform.localEulerAngles.z > maxRotateBoundary)
            myRenderer.sortingOrder = (int)(sortingOrderBase - transform.parent.position.y * 100 - offset - 2);
        else
            myRenderer.sortingOrder = (int)(sortingOrderBase - transform.parent.position.y * 100 - offset);
    }
}
