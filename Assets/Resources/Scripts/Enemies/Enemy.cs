using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float healthRegenRate;
    public float maxHealthRegenRate;

    public enum DamageOverTimeType { FIRE, POISON, BLEED }

    private float fireTimeLeft, poisonTimeLeft, bleedTimeLeft;
    private float fireDamage, poisonDamage, bleedDamage;

    public GameObject healthBar;
    public Slider healthBarSlider;

    public Rigidbody2D rb;
    public GameObject onFire;

    public GameObject coin, exp;
    public int coinAmount, expAmount;
    private Vector3 coinPos, expPos;

    public string hitSound;
    public string deathSound;

    virtual protected void Start()
    {
        health = maxHealth;
    }

    virtual protected void Update()
    {
        healthRegenRate = maxHealthRegenRate - fireDamage - poisonDamage - bleedDamage;
        health += healthRegenRate * Time.deltaTime;
        if (fireTimeLeft > 0)
        {
            fireTimeLeft -= Time.deltaTime;
            if (fireTimeLeft <= 0)
            {
                fireDamage = 0;
                onFire.SetActive(false);
            }                         
        }
        if (poisonTimeLeft > 0)
        {
            poisonTimeLeft -= Time.deltaTime;
            if (poisonTimeLeft <= 0)
            {
                poisonDamage = 0;
            }                
        }
        if (bleedTimeLeft > 0)
        {
            bleedTimeLeft -= Time.deltaTime;
            if (bleedTimeLeft <= 0)
            {
                bleedDamage = 0;
            }                
        }
        if (healthRegenRate != 0)
        {
            healthBarSlider.value = CalculateHealthPercentage();
            CheckDeath();
            CheckOverheal();
        }            
    }

    // straight damage
    public void DealDamage(float damage)
    {
        healthBar.SetActive(true);
        health -= damage;
        CheckDeath();
        healthBarSlider.value = CalculateHealthPercentage();        
    }

    // damage with damage over time
    public void DealDamage(float damage, float damageOverTimeAmount, float dotTime, DamageOverTimeType dotType)
    {
        healthBar.SetActive(true);
        health -= damage;
        CheckDeath();
        healthBarSlider.value = CalculateHealthPercentage();

        switch (dotType)
        {
            case DamageOverTimeType.FIRE:
                onFire.SetActive(true);
                fireDamage = damageOverTimeAmount;
                fireTimeLeft = dotTime;
                break;
            case DamageOverTimeType.POISON:
                //onFire.SetActive(true);
                poisonDamage = damageOverTimeAmount;
                poisonTimeLeft = dotTime;
                break;
            case DamageOverTimeType.BLEED:
                //onFire.SetActive(true);
                bleedDamage = damageOverTimeAmount;
                bleedTimeLeft = dotTime;
                break;
        }
    }

    // damage with knockback
    public void DealDamage(float damage, Vector2 hitPos, float knockback)
    {
        healthBar.SetActive(true);
        health -= damage;
        CheckDeath();
        healthBarSlider.value = CalculateHealthPercentage();

        Vector2 recoilForce = (new Vector2(transform.position.x, transform.position.y) - hitPos).normalized * Mathf.Pow(knockback, 2);
        rb.AddForce(recoilForce);
    }

    // damage with knockback and damage over time
    public void DealDamage(float damage, Vector2 hitPos, float knockback, float damageOverTimeAmount, float dotTime, DamageOverTimeType dotType)
    {
        healthBar.SetActive(true);
        health -= damage;
        CheckDeath();
        healthBarSlider.value = CalculateHealthPercentage();

        Vector2 recoilForce = (new Vector2(transform.position.x, transform.position.y) - hitPos).normalized * Mathf.Pow(knockback, 2);
        rb.AddForce(recoilForce);

        switch (dotType)
        {
            case DamageOverTimeType.FIRE:
                onFire.SetActive(true);
                fireDamage = damageOverTimeAmount;
                fireTimeLeft = dotTime;
                break;
            case DamageOverTimeType.POISON:
                //onFire.SetActive(true);
                poisonDamage = damageOverTimeAmount;
                poisonTimeLeft = dotTime;
                break;
            case DamageOverTimeType.BLEED:
                //onFire.SetActive(true);
                bleedDamage = damageOverTimeAmount;
                bleedTimeLeft = dotTime;
                break;
        }
    }

    public void HealCharacter(float heal)
    {
        health += heal;
        CheckOverheal();
        healthBarSlider.value = CalculateHealthPercentage();
    }

    private void CheckOverheal()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            FindObjectOfType<AudioManager>().Play(deathSound);
            PlayerStats.playerStats.enemiesKilled++;
            PlayerStats.playerStats.enemiesKilledText.text = "Enemies Killed: " + PlayerStats.playerStats.enemiesKilled;
            Destroy(gameObject);
            for (int i = 0; i < coinAmount; i++)
            {
                coinPos = transform.position;
                Instantiate(coin, coinPos, Quaternion.identity);
            }
            for (int i = 0; i < expAmount; i++)
            {
                expPos = transform.position;
                Instantiate(exp, expPos, Quaternion.identity);
            }
        }
        else
            FindObjectOfType<AudioManager>().Play(hitSound);
    }

    private float CalculateHealthPercentage()
    {
        return health / maxHealth;
    }

}
