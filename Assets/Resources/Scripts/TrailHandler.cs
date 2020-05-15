using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailHandler : MonoBehaviour
{
    public Player.Debuff debuff1, debuff2;
    public float debuff1Time, debuff2Time;
    public float minDamage, maxDamage;
    public float collisionCooldown;
    private float cooldownTimer;

    private void Start()
    {
        cooldownTimer = 0f;
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (cooldownTimer < 0f)
            {
                cooldownTimer = collisionCooldown;
                collision.GetComponent<Player>().DealDamage(Random.Range(minDamage, maxDamage));
                collision.GetComponent<Player>().InflictDebuff(debuff1, debuff1Time);
                collision.GetComponent<Player>().InflictDebuff(debuff2, debuff2Time);
            }
        }
    }
}
