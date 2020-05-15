using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile
{
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
                    collision.GetComponent<Enemy>().DealDamage(damage, transform.position, knockback);
                    collision.GetComponent<Enemy>().InflictDebuff(Enemy.Debuff.BURN, damageOverTimeLength);
                }
                if (collision.tag == "Player")
                {
                    PlayerStats.playerStats.GetPlayer().DealDamage(damage, transform.position, knockback);
                    PlayerStats.playerStats.GetPlayer().InflictDebuff(Player.Debuff.BURN, damageOverTimeLength);
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
        GameObject particles = transform.GetComponentInChildren<ParticleSystem>().gameObject;
        if (transform.GetComponentInChildren<ParticleSystem>() != null)
        {
            Destroy(particles, 0.15f);
        }
        transform.DetachChildren();
        Destroy(gameObject);
    }
}
