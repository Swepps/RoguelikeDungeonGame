using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float damage;
    public float range;
    public float knockback;
    protected Vector2 startPos;
    public string[] collisionTags;
    public string shootSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PerformanceStats.collisions++;
        for (int i = 0; i < collisionTags.Length; i++)
        {
            if (collision.tag == collisionTags[i])
            {
                if (collision.tag == "Enemy")
                {
                    collision.GetComponent<Enemy>().DealDamage(damage, transform.position, knockback);
                }
                if (collision.tag == "Player")
                {
                    PlayerStats.playerStats.DealDamage(damage);
                }
                Destroy(gameObject);
                break;
            }
        }
    }

    private void Start()
    {
        startPos = transform.position;
        knockback = PlayerStats.playerStats.stats.knockback;
        range = PlayerStats.playerStats.stats.projectileRange;
        FindObjectOfType<AudioManager>().Play(shootSound);
    }

    virtual protected void Update()
    {
        if (Vector2.Distance(startPos, transform.position) > range)
            Destroy(gameObject);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
