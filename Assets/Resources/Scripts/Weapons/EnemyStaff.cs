using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStaff : EnemyWeapon
{
    public float distanceFromCenter;

    override protected void Start()
    {
        base.Start();
    }

    override protected void Update()
    {
        FlipStaff();
    }

    protected void FlipStaff()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.parent.position).normalized;
            if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            Vector3 newPos = transform.parent.position;
            newPos.x += direction.x * distanceFromCenter;
            newPos.y += direction.y * distanceFromCenter;
            transform.position = newPos;
        }
    }
}
