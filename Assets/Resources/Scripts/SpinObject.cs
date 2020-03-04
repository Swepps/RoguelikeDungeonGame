using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.Rotate(Vector3.back * speed * Time.deltaTime);
    }
}
