using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public List<GameObject> projectiles;
    private int projectileNum = 0;
    private Transform target;

    public float minDamage;
    public float maxDamage;
    public float projectileForce;
    public float cooldown;

    private bool CR_Running = false;
    private bool isPaused = false;

    public void StartShooting(Transform target)
    {
        if (!CR_Running)
        {
            this.target = target;
            StartCoroutine(ShootTarget());
        }
    }

    public void StopShooting()
    {
        CR_Running = false;
    }

    public void PauseShooting()
    {
        isPaused = true;
    }

    public void ResumeShooting()
    {
        isPaused = false;
    }

    public bool IsShooting()
    {
        return CR_Running;
    }

    IEnumerator ShootTarget()
    {
        CR_Running = true;
        while (CR_Running)
        {
            if (projectileNum > projectiles.Count - 1)
                projectileNum = 0;
            GameObject projectileClone = Instantiate(projectiles[projectileNum], transform.position, Quaternion.identity);
            projectileNum++;
            Vector2 myPos = transform.position;
            Vector2 targetPos = target.position;
            Vector2 direction = (targetPos - myPos).normalized;
            projectileClone.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
            projectileClone.GetComponent<Projectile>().SetDamage(Random.Range(minDamage, maxDamage));    

            while (isPaused)
            {
                yield return null;
            }

            yield return new WaitForSeconds(cooldown);
        }
    }
}
