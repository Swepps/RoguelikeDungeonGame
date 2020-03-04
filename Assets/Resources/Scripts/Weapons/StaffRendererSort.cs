using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffRendererSort : PositionRendererSort
{
    override protected void LateUpdate()
    {
        if (transform.localPosition.y > 0)
            myRenderer.sortingOrder = (int)(sortingOrderBase - transform.parent.position.y * 100 - offset - 2);
        if (transform.localPosition.y < 0)
            myRenderer.sortingOrder = (int)(sortingOrderBase - transform.parent.position.y * 100 - offset);
    }
}
