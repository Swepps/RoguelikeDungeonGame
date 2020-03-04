using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon
{

    override protected void Start()
    {
        base.Start();
    }

    override protected void Update()
    {
        FlipStaff();
        if (Input.GetMouseButton(0) && shootReady)
        {
            shootReady = false;
            ShootProjectile(new Vector2(transform.position.x, transform.position.y + 0.5f));
            StartCoroutine(Cooldown());
        }
    }

    protected void FlipStaff()
    {
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position).normalized;
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        Vector3 newPos = transform.parent.position;
        newPos.x += direction.x / 3;
        newPos.y += direction.y / 3 - 0.025f;
        transform.position = newPos;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f / attacksPerSecond);
        shootReady = true;
    }
}
