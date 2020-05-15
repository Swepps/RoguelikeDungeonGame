using UnityEngine;
using System.Collections;

public class ThrowProjectile : SpecialAttack
{
    public GameObject projectile;
    private SpriteRenderer projSprite;
    public float projectileRotationOffset, projectileForce, minDamage, maxDamage;
    private float shotAngle, angle;
    public int shotSpread;
    public float projKnockback;

    public void Initialise()
    {
        if (projectile.name != "Fireball")
        {
            projSprite = projectile.GetComponent<SpriteRenderer>();
            projSprite.sprite = Resources.Load<Sprite>("Sprites/weapon_" + PlayerStats.playerStats.stats.weaponSprite);
        }
        projectile.GetComponent<Projectile>().knockback = projKnockback;
    }

    protected override bool Special()    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        Vector3 spawnPos = transform.position;
        spawnPos.x += direction.x * 0.6f;
        spawnPos.y += direction.y * 0.6f;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (shotSpread > 1)
        {
            shotAngle = -10.0f * shotSpread / 2;
            for (int i = 0; i < shotSpread; i++)
            {
                GameObject newProjectile = Instantiate(projectile, spawnPos, Quaternion.AngleAxis(angle + projectileRotationOffset + shotAngle, Vector3.forward));
                newProjectile.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
                newProjectile.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(shotAngle, Vector3.forward) * newProjectile.GetComponent<Rigidbody2D>().velocity;
                newProjectile.GetComponent<Projectile>().SetDamage(Random.Range(minDamage, maxDamage));
                shotAngle += 10.0f;
            }
        }
        else
        {
            GameObject newProjectile = Instantiate(projectile, spawnPos, Quaternion.AngleAxis(angle + projectileRotationOffset, Vector3.forward));
            newProjectile.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            newProjectile.GetComponent<Projectile>().SetDamage(Random.Range(minDamage, maxDamage));
        }
        return true;
    }

}
