using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyProjectile : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PerformanceStats.collisions++;
        if (collision.tag != "Enemy" && collision.tag != "Projectile" && collision.tag != "Currency" && collision.tag != "EnemyProjectile")
        {
            if (collision.tag == "Player")
            {
                PlayerStats.playerStats.DealDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
