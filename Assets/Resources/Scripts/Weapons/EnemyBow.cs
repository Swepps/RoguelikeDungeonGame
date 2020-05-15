using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBow : EnemyWeapon
{
    private Animator bowAnimator;
    private bool isShooting;

    override protected void Start()
    {
        base.Start();
        bowAnimator = GetComponent<Animator>();
        bowAnimator.SetFloat("shotsPerSecond", attacksPerSecond);
    }

    override protected void Update()
    {
        base.Update();
    }

    public void ShootReady()
    {
        shootReady = true;
    }

    public void ShootProjectile()
    {
        Vector2 targetPos = target.position;
        Vector2 spawnPos = transform.position;
        Vector2 direction = (targetPos - spawnPos).normalized;
        spawnPos.x += direction.x * 0.3f;
        spawnPos.y += direction.y * 0.3f;
        GameObject projectileClone = Instantiate(projectile, spawnPos, Quaternion.AngleAxis(angle + projectileRotationOffset, Vector3.forward));
        projectileClone.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
        projectileClone.GetComponent<Projectile>().SetDamage(Random.Range(minDamage, maxDamage));
    }

    override public void StartShooting(Transform target)
    {
        this.target = target;
        SetAnimatorVars(true);
        isShooting = true;
    }

    public override void StopShooting()
    {
        SetAnimatorVars(false);
        isShooting = false;
    }

    public override bool IsShooting()
    {
        return isShooting;
    }

    private void SetAnimatorVars(bool isShooting)
    {
        if (isShooting)
        {
            bowAnimator.SetBool("isShooting", true);
        }
        else
        {
            bowAnimator.SetBool("isShooting", false);
        }
    }
}
