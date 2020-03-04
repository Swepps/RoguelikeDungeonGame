using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject projectile;
    public float rotateOffset;
    public float projectileRotationOffset;

    protected SpriteRenderer spriteRenderer;

    protected float minDamage;
    protected float maxDamage;
    protected float attacksPerSecond;
    protected float projectileForce;
    protected float knockback;

    protected int shotSpread;

    protected float angle;
    protected bool shootReady;

    virtual protected void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        UpdateStats();
    }

    virtual public void UpdateStats()
    {
        shootReady = true;
        minDamage = PlayerStats.playerStats.stats.damageMin;
        maxDamage = PlayerStats.playerStats.stats.damageMax;
        attacksPerSecond = PlayerStats.playerStats.stats.attacksPerSecond;
        projectileForce = PlayerStats.playerStats.stats.projectileForce;
        knockback = PlayerStats.playerStats.stats.knockback;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/weapon_" + PlayerStats.playerStats.stats.weaponSprite);
    }

    virtual protected void Update()
    {
        RotateWeapon();
    }

    virtual public void ShootProjectile()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        Vector3 spawnPos = transform.position;
        spawnPos.x += direction.x;
        spawnPos.y += direction.y;
        GameObject newProjectile = Instantiate(projectile, spawnPos, Quaternion.AngleAxis(angle + projectileRotationOffset, Vector3.forward));
        newProjectile.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
        newProjectile.GetComponent<Projectile>().SetDamage(Random.Range(minDamage, maxDamage));
    }

    virtual public void ShootProjectile(Vector2 spawnPos)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        GameObject newProjectile = Instantiate(projectile, spawnPos, Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.up, direction), Vector3.forward));
        newProjectile.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
        newProjectile.GetComponent<Projectile>().SetDamage(Random.Range(minDamage, maxDamage));
    }

    protected void RotateWeapon()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - rotateOffset;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
