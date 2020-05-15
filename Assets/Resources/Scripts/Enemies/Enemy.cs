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

    public enum Debuff { BURN, POISON, BLEED, SLOW, PARALYSE, NONE }

    private float burnTime, poisonTime, bleedTime, slowTime, paralyseTime;
    public float burnDamage, poisonDamage, bleedDamage;

    public GameObject burned, poisoned, bleeding, slowed, paralysed;

    public GameObject healthBar;
    public Slider healthBarSlider;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;
    private EnemyPathfinding enemyPathfinding;

    public GameObject coin, exp, healthItem, key;
    public int coinAmount, expAmount;
    public float healthDropChance;
    private Vector3 coinPos, expPos, healthPos;

    public float touchDamageMin;
    public float touchDamageMax;
    public float knockback;

    public string hitSound;
    public string deathSound;
        
    [HideInInspector]
    public Room room;

    protected void Start()
    {
        health = maxHealth;
        room = GameObject.FindGameObjectWithTag("RoomSpawner").GetComponent<RoomSpawner>().roomGrid.GetGridObject(transform.position);
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyPathfinding = GetComponent<EnemyPathfinding>();
    }

    protected void Update()
    {
        if (enemyPathfinding != null)
            animator.SetFloat("moveSpeed", enemyPathfinding.speed * 0.4f);
        enemyPathfinding.speed = enemyPathfinding.maxSpeed;
        healthRegenRate = maxHealthRegenRate;

        if (burnTime > 0)
        {
            burnTime -= Time.deltaTime;
            healthRegenRate -= burnDamage;
            if (burnTime <= 0)
            {
                burned.SetActive(false);
            }
        }
        if (poisonTime > 0)
        {
            healthRegenRate -= poisonDamage * (5 / (poisonTime + 1));
            poisonTime -= Time.deltaTime;
            if (poisonTime <= 0)
            {
                poisoned.SetActive(false);
            }
        }
        if (bleedTime > 0)
        {
            bleedTime -= Time.deltaTime;
            healthRegenRate -= bleedDamage * burnTime;
            if (bleedTime <= 0)
            {
                bleeding.SetActive(false);
            }
        }
        if (slowTime > 0)
        {
            slowTime -= Time.deltaTime;
            if (enemyPathfinding != null)
            {
                enemyPathfinding.speed = enemyPathfinding.maxSpeed * 0.5f;
            }
            if (slowTime <= 0)
            {
                //slowed.SetActive(false);
            }
        }
        if (paralyseTime > 0)
        {
            paralyseTime -= Time.deltaTime;
            if (enemyPathfinding != null)
            {
                enemyPathfinding.speed = 0;
            }
            if (paralyseTime <= 0)
            {
                //paralysed.SetActive(false);
            }
        }

        health += healthRegenRate * Time.deltaTime;        

        if (!rb.IsSleeping())
            animator.SetBool("isRunning", true);        
        else
            animator.SetBool("isRunning", false);
    }

    public void SetSpriteDirection(Vector2 direction)
    {
        if (direction.x < 0)
            spriteRenderer.flipX = true;
        else if (direction.x > 0)
            spriteRenderer.flipX = false;
    }

    // straight damage
    public void DealDamage(float damage)
    {
        healthBar.SetActive(true);
        health -= damage;
        CheckDeath();
        healthBarSlider.value = CalculateHealthPercentage();        
    }

    // damage with knockback
    public void DealDamage(float damage, Vector2 hitPos, float knockback)
    {
        healthBar.SetActive(true);
        health -= damage;
        CheckDeath();
        healthBarSlider.value = CalculateHealthPercentage();

        Vector2 recoilForce = (new Vector2(transform.position.x, transform.position.y) - hitPos).normalized * knockback;
        rb.AddForce(recoilForce, ForceMode2D.Impulse);
    }

    public void InflictDebuff(Debuff debuff, float time)
    {
        switch (debuff)
        {
            case Debuff.BURN:
                burned.SetActive(true);
                burnTime = time;
                break;
            case Debuff.POISON:
                poisoned.SetActive(true);
                poisonTime = time;
                break;
            case Debuff.BLEED:
                bleeding.SetActive(true);
                bleedTime = time;
                break;
            case Debuff.SLOW:
                slowTime = time;
                break;
            case Debuff.PARALYSE:
                paralyseTime = time;
                break;
            default:
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
            room.enemiesRemaining--;
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
            for (float i = 0; i < healthDropChance; i++)
            {
                if (Random.value < healthDropChance - i)
                {
                    healthPos = transform.position;
                    Instantiate(healthItem, healthPos, Quaternion.identity);
                }
            }
            if (key != null)
            {
                Instantiate(key, transform.position, Quaternion.identity);
            }
        }
        else
            FindObjectOfType<AudioManager>().Play(hitSound);
    }

    private float CalculateHealthPercentage()
    {
        return health / maxHealth;
    }

    public void SetRoom(Room room)
    {
        this.room = room;
    }

    public Room GetRoom()
    {
        return room;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats.playerStats.GetPlayer().DealDamage(Random.Range(touchDamageMin, touchDamageMax), transform.position, knockback);
        }
    }
}
