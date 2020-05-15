using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;
    private GameObject bow, sword, staff;
    public GameObject thrownSword, specialFireball;
    private SpecialAttack oldSpecial;
    private SpecialAttack special;

    public enum Debuff { BURN, POISON, BLEED, SLOW, PARALYSE, WEAK, NONE }

    private float burnTime, poisonTime, bleedTime, slowTime, paralyseTime, weakTime;
    public float burnDamage, poisonDamage, bleedDamage;

    private Rigidbody2D rb;
    public GameObject burned, poisoned, bleeding, slowed, paralysed, weakened;

    private PlayerStats stats;
    private float minDamage, maxDamage;

    void Start()
    {       
        playerMovement.rb.freezeRotation = true;
        playerMovement.lockMovement = false;
        bow = gameObject.transform.GetChild(0).gameObject;
        sword = gameObject.transform.GetChild(1).gameObject;
        staff = gameObject.transform.GetChild(2).gameObject;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            stats.stats.invulnerable = !stats.stats.invulnerable;
            Debug.Log("Invulnerable: " + stats.stats.invulnerable);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            stats.experience = stats.levels[stats.level];
            stats.SetExpUI();
        }
        stats.healthRegen = stats.stats.healthRegenRate;
        if (playerMovement.moveSpeed < playerMovement.maxSpeed)
            playerMovement.moveSpeed = playerMovement.maxSpeed;
        stats.stats.damageMin = minDamage;
        stats.stats.damageMax = maxDamage;
        if (burnTime > 0)
        {
            burnTime -= Time.deltaTime;
            stats.healthRegen = stats.healthRegen - burnDamage;
            if (burnTime <= 0)
            {                
                burned.SetActive(false);
            }
        }
        if (poisonTime > 0)
        {            
            stats.healthRegen = stats.healthRegen - poisonDamage * (5 / (poisonTime + 1));
            poisonTime -= Time.deltaTime;
            if (poisonTime <= 0)
            {                
                poisoned.SetActive(false);
            }
        }
        if (bleedTime > 0)
        {
            bleedTime -= Time.deltaTime;
            stats.healthRegen = stats.healthRegen - bleedDamage;
            if (bleedTime <= 0)
            {                
                bleeding.SetActive(false);
            }
        }
        if (slowTime > 0)
        {
            slowTime -= Time.deltaTime;
            playerMovement.moveSpeed = playerMovement.maxSpeed * 0.5f;
        }
        if (paralyseTime > 0)
        {
            paralyseTime -= Time.deltaTime;
            playerMovement.moveSpeed = 0f;
            if (paralyseTime <= 0)
            {
                spriteRenderer.color = Color.white;                                
            }
        }
        if (weakTime > 0)
        {
            weakTime -= Time.deltaTime;
            stats.stats.damageMin = minDamage * 0.5f;
            stats.stats.damageMax = maxDamage * 0.5f;
            if (weakTime <= 0)
            {
                weakened.SetActive(false);
            }
        }


        if (special != null)
            special.ManualUpdate();
    }

    public void UpdateStats()
    {
        stats = PlayerStats.playerStats;
        playerMovement.maxSpeed = stats.stats.moveSpeed;
        playerMovement.moveSpeed = playerMovement.maxSpeed;
        minDamage = stats.stats.damageMin;
        maxDamage = stats.stats.damageMax;
        SetWeapon(stats.stats.weapon);
        PlayerStats.playerStats.health = PlayerStats.playerStats.stats.maxHealth;
        PlayerStats.playerStats.ability = PlayerStats.playerStats.stats.maxAbility;
        oldSpecial = gameObject.GetComponent<SpecialAttack>();
        Destroy(oldSpecial);
        SetSpecial(stats.stats.special);        
        playerMovement.animator.runtimeAnimatorController = Resources.Load("Animation/" + stats.stats.character + "_movement_controller") as RuntimeAnimatorController;
        playerMovement.animator.SetFloat("moveSpeed", stats.stats.moveSpeed / 3f);
    }

    private void SetWeapon(SkillPath.Weapon weapon)
    {
        if (weapon == SkillPath.Weapon.BOW)
        {
            bow.SetActive(true);
            sword.SetActive(false);
            staff.SetActive(false);
        }
        else if (weapon == SkillPath.Weapon.SWORD)
        {
            bow.SetActive(false);
            sword.SetActive(true);
            staff.SetActive(false);
        }
        else
        {
            bow.SetActive(false);
            sword.SetActive(false);
            staff.SetActive(true);
        }
    }

    private void SetSpecial(SkillPath.Special special)
    {
        switch (special)
        {
            case SkillPath.Special.DASH:
                gameObject.AddComponent<Dash>();
                this.special = GetComponent<Dash>();
                break;
            case SkillPath.Special.THROWSWORD:
                gameObject.AddComponent<ThrowProjectile>();
                gameObject.GetComponent<ThrowProjectile>().projectile = thrownSword;
                gameObject.GetComponent<ThrowProjectile>().projectileForce = 4;
                gameObject.GetComponent<ThrowProjectile>().minDamage = 10;
                gameObject.GetComponent<ThrowProjectile>().maxDamage = 35;
                gameObject.GetComponent<ThrowProjectile>().shotSpread = 1;
                gameObject.GetComponent<ThrowProjectile>().projKnockback = 4f;
                gameObject.GetComponent<ThrowProjectile>().Initialise();
                this.special = GetComponent<ThrowProjectile>();
                break;
            case SkillPath.Special.FIREBALL:
                gameObject.AddComponent<ThrowProjectile>();
                gameObject.GetComponent<ThrowProjectile>().projectile = specialFireball;
                gameObject.GetComponent<ThrowProjectile>().projectileForce = 6;
                gameObject.GetComponent<ThrowProjectile>().minDamage = 10;
                gameObject.GetComponent<ThrowProjectile>().maxDamage = 20;
                gameObject.GetComponent<ThrowProjectile>().shotSpread = 1;
                gameObject.GetComponent<ThrowProjectile>().projKnockback = 0.5f;
                gameObject.GetComponent<ThrowProjectile>().Initialise();
                this.special = GetComponent<ThrowProjectile>();
                break;
            default:
                break;
        }        
    }

    // straight damage
    public void DealDamage(float damage)
    {
        PlayerStats.playerStats.DealDamage(damage);
    }

    // damage with knockback
    public void DealDamage(float damage, Vector2 hitPos, float knockback)
    {
        PlayerStats.playerStats.DealDamage(damage);
        Vector2 recoilForce = (new Vector2(transform.position.x, transform.position.y) - hitPos).normalized * knockback;
        playerMovement.Recoil(recoilForce);
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
                spriteRenderer.color = new Color(0.4f, 0.8f, 1f, 1);
                paralyseTime = time;
                break;
            case Debuff.WEAK:
                weakTime = time;
                break;
            default:
                break;
        }
    }
}
