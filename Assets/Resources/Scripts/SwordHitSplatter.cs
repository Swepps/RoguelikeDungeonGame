using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SwordHitSplatter : MonoBehaviour
{
    public GameObject bloodSplatter;
    public string collisionTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PerformanceStats.collisions++;
        if (collision.tag == collisionTag)
        {
            Vector2 swingDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position;
            swingDir.Normalize();
            Vector3 splatterSpawnPos = gameObject.transform.position;
            splatterSpawnPos.x += swingDir.x;
            splatterSpawnPos.y += swingDir.y;
            GameObject splatterClone = Instantiate(bloodSplatter, splatterSpawnPos, transform.rotation);
            Destroy(splatterClone, 1.0f);
        }
        
    }
}
