using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public string[] collisionTags;
    private BoxCollider2D swordCollider;
    private TrailRenderer trailRenderer;
    private Vector2 swingDir;
    private bool swinging;
    private float swingTime;
    public float swingSpeed = 3.0f;
    public string swingSound;

    private new void Start()
    {
        base.Start();
        spriteRenderer.enabled = false;
        swordCollider = gameObject.GetComponent<BoxCollider2D>();
        swordCollider.enabled = false;
        trailRenderer = transform.GetChild(0).gameObject.GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
        swinging = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PerformanceStats.collisions++;
        for (int i = 0; i < collisionTags.Length; i++)
        {
            if (collision.tag == collisionTags[i])
            {
                if (collision.tag == "Enemy")
                {
                    collision.GetComponent<Enemy>().DealDamage(Random.Range(minDamage, maxDamage), transform.position, knockback);
                }
                break;
            }
        }
    }

    override protected void Update()
    {        
        if (Input.GetMouseButton(0) && shootReady)
        {
            swinging = true;
            swingTime = 0;
            shootReady = false;
            trailRenderer.enabled = true;
            spriteRenderer.enabled = true;
            swordCollider.enabled = true;            
            swingDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position;
            angle = Mathf.Atan2(swingDir.y, swingDir.x) * Mathf.Rad2Deg - rotateOffset + 80;
            StartCoroutine(Cooldown());
            FindObjectOfType<AudioManager>().Play(swingSound);
        }
        if (swinging)
        {
            Swing();
        }
    }

    private void Swing()
    {
        swingTime += Time.deltaTime;
        if (swingTime * swingSpeed >= 1.0f)
        {
            swinging = false;
            trailRenderer.enabled = false;
            spriteRenderer.enabled = false;
            swordCollider.enabled = false;            
        }            
        else
            transform.rotation = Quaternion.Slerp(Quaternion.AngleAxis(angle, Vector3.forward), Quaternion.AngleAxis(angle - 160, Vector3.forward), swingTime * swingSpeed);
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f/attacksPerSecond);
        shootReady = true;
    }
}
