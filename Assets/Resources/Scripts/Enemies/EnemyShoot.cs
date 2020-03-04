using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : Enemy
{
    public GameObject projectile;
    private GameObject player;
    private Transform playerPos;
    public float minDamage;
    public float maxDamage;
    public float projectileForce;
    public float cooldown;

    override protected void Start()
    {
        base.Start();
        player = PlayerStats.playerStats.GetPlayer();

        if (player != null)
        {
            playerPos = player.transform;
            StartCoroutine(ShootPlayer());
        }      
    }

    IEnumerator ShootPlayer()
    {
        while (player != null)
        {
            GameObject spell = Instantiate(projectile, transform.position, Quaternion.identity);
            Vector2 myPos = transform.position;
            Vector2 targetPos = playerPos.position;
            Vector2 direction = (targetPos - myPos).normalized;
            spell.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            spell.GetComponent<TestEnemyProjectile>().damage = Random.Range(minDamage, maxDamage);
            yield return new WaitForSeconds(cooldown);
        }
    }
}
