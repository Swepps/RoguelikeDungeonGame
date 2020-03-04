using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectileCollision : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PerformanceStats.collisions++;
        if (collision.tag != "Player" && collision.tag != "Projectile" && collision.tag != "EnemyProjectile" && collision.tag != "Currency")
        {
            if (collision.GetComponent<Enemy>() != null)
            {
                collision.GetComponent<Enemy>().DealDamage(damage, collision.transform.position, 10);
            }
            Destroy(gameObject);
        }
    }
}
