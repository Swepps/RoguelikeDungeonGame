using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HitSplatter : MonoBehaviour
{
    public GameObject bloodSplatter;
    public string collisionTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PerformanceStats.collisions++;
        if (collision.tag == collisionTag)
        {
            GameObject splatterClone = Instantiate(bloodSplatter, collision.gameObject.transform.position, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, gameObject.GetComponent<Rigidbody2D>().velocity.normalized)));
            Destroy(splatterClone, 1.0f);
        }
        
    }
}
