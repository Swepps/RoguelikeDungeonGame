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
    public Enemy.Debuff enemyDebuff;
    public Player.Debuff playerDebuff;
    public float debuffTime;
    public bool playerProjectile;

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
                    collision.GetComponent<Enemy>().InflictDebuff(enemyDebuff, debuffTime);
                }
                if (collision.tag == "Player")
                {
                    Debug.Log(knockback);
                    PlayerStats.playerStats.GetPlayer().DealDamage(damage, transform.position, knockback);
                    PlayerStats.playerStats.GetPlayer().InflictDebuff(playerDebuff, debuffTime);
                }
                Destroy(gameObject);
                break;
            }
        }
    }

    private void Start()
    {
        startPos = transform.position;        
        if (playerProjectile)
        {
            knockback = PlayerStats.playerStats.stats.knockback;
            range = PlayerStats.playerStats.stats.projectileRange;
        }
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
