using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRendererSort : MonoBehaviour
{
    [SerializeField]
    protected int sortingOrderBase = 10000;
    [SerializeField]
    protected int offset = 0;
    protected Renderer myRenderer;

    private void Awake()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
    }

    virtual protected void LateUpdate()
    {
        myRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y * 100 - offset);
    }
}
