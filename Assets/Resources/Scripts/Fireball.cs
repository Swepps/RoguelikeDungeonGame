using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile
{
    public float damageOverTimeAmount;
    public float damageOverTimeLength;
    public string impactSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PerformanceStats.collisions++;
        //FindObjectOfType<AudioManager>().Play(impactSound);
        for (int i = 0; i < collisionTags.Length; i++)
        {
            if (collision.tag == collisionTags[i])
            {
                if (collision.tag == "Enemy")
                {
                    collision.GetComponent<Enemy>().DealDamage(damage, transform.position, knockback, damageOverTimeAmount, damageOverTimeLength, Enemy.DamageOverTimeType.FIRE);
                }
                if (collision.tag == "Player")
                {
                    PlayerStats.playerStats.DealDamage(damage);
                }
                DestroyObject();
                break;
            }
        }
    }

    override protected void Update()
    {
        if (Vector2.Distance(startPos, transform.position) > range)
            DestroyObject();
    }

    protected void DestroyObject()
    {
        GameObject particles = transform.GetChild(0).gameObject;
        Destroy(particles, 0.15f);
        transform.DetachChildren();
        Destroy(gameObject);
    }
}
