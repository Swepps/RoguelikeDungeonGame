using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public GameObject projectile;
    public float rotateOffset;
    public float projectileRotationOffset;

    protected SpriteRenderer spriteRenderer;

    public float minDamage;
    public float maxDamage;
    public float attacksPerSecond;
    public float projectileForce;
    public float knockback;
    public int shotSpread;

    protected float angle;
    protected bool shootReady;

    protected Transform target;
    protected bool CR_Running = false;
    protected bool isPaused = false;

    virtual protected void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        shootReady = true;
    }

    virtual protected void Update()
    {
        RotateWeapon();
    }

    virtual public void StartShooting(Transform target)
    {
        if (!CR_Running)
        {
            this.target = target;
            StartCoroutine(ShootTarget());
        }
    }

    virtual public void StopShooting()
    {
        CR_Running = false;
    }

    virtual public bool IsShooting()
    {
        return CR_Running;
    }

    protected IEnumerator ShootTarget()
    {
        CR_Running = true;
        while (CR_Running)
        {                       
            Vector2 targetPos = target.position;            
            Vector2 spawnPos = transform.position;
            Vector2 direction = (targetPos - spawnPos).normalized;
            spawnPos.x += direction.x;
            spawnPos.y += direction.y;
            GameObject projectileClone = Instantiate(projectile, spawnPos, Quaternion.identity);
            projectileClone.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            projectileClone.GetComponent<Projectile>().SetDamage(Random.Range(minDamage, maxDamage));

            while (isPaused)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1f / attacksPerSecond);
        }
    }

    protected void RotateWeapon()
    {
        if (target != null)
        {
            Vector2 direction = target.position - transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - rotateOffset;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
