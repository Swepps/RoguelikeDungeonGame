using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PerformanceStats.collisions++;
        if (collision.tag == "Player")
        {
            PlayerStats.playerStats.DealDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.tag == "Walls")
            Destroy(gameObject);        
    }
}
