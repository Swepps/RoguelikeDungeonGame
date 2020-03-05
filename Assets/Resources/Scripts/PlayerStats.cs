using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats playerStats;

    public GameObject playerPrefab;
    private GameObject localPlayer;
    public Text healthText;
    public Slider healthSlider;
    public Text abilityText;
    public Slider abilitySlider;
    public Text coinsText;
    public Slider expSlider;
    public Image expFill;
    public Text enemiesKilledText;
    public PauseMenu pauseMenu;

    public SkillPath stats;

    public float health;
    public float ability;    

    public int enemiesKilled;

    public int level, path;
    public int[] levels = { 30, 5000};
    public float experience;
    public int coins;


    // also run when player is created??
    private void Awake()
    {
        if (playerStats != null)
        {
            Destroy(playerStats);
        }
        else
        {
            playerStats = this;
        }
    }

    // run when player is created
    private void Start()
    {
        SkillPath.InitialiseSkillPaths();
        stats = SkillPath.skillPaths[0];

        health = stats.maxHealth;
        ability = 0;
        level = 0;
        path = 0;
        SetHealthUI();
        SetAbilityUI();
        SetExpUI();
        stats.invulnerable = false;
        localPlayer = Instantiate(playerPrefab, Vector2.zero, Quaternion.identity);        
    }

    private void Update()
    {
        if (ability < stats.maxAbility)
            ability += stats.abilityChargeRate * Time.deltaTime;
        else
            ability = stats.maxAbility;
        SetAbilityUI();

        if (health < stats.maxHealth)
            health += stats.healthRegenRate * Time.deltaTime;
        else
            health = stats.maxHealth;
        SetHealthUI();
    }

    // damages the player
    public void DealDamage(float damage)
    {
        if (!stats.invulnerable)
        {            
            health -= damage;
            CheckDeath();
            SetHealthUI();
        }        
    }

    // heals the player
    public void HealCharacter(float heal)
    {
        health += heal;
        CheckOverheal();
        SetHealthUI();
    }

    // prevents overheal
    private void CheckOverheal()
    {
        if (health > stats.maxHealth)
        {
            health = stats.maxHealth;
        }
    }

    // destroys the player if their health goes below 0
    private void CheckDeath()
    {
        if (health <= 0)
        {
            FindObjectOfType<AudioManager>().Play("PlayerDeath");
            health = 0;
            Destroy(localPlayer);
            SceneManager.LoadScene(0);
        }
        else
            FindObjectOfType<AudioManager>().Play("PlayerHit");
    }

    // Sets the UI health bar and text
    private void SetHealthUI()
    {
        healthSlider.value = CalculateHealthPercentage();
        healthText.text = Mathf.Ceil(health) + " / " + Mathf.Ceil(stats.maxHealth);
    }

    // Sets the UI ability bar and text
    private void SetAbilityUI()
    {
        if (stats.abilityChargeRate == 0)
        {
            abilityText.text = "Disabled";
        }
        else
        {
            abilitySlider.value = ability / stats.maxAbility;
            if (ability >= stats.abilityCost)
                abilityText.text = "Ready!";
            else
                abilityText.text = "Charging...";
        }
    }

    // Sets the exp bar UI
    public void SetExpUI()
    {
        if (level < levels.Length - 1)
        {
            if (experience >= levels[level])
            {
                pauseMenu.LevelUpWindow();
                level++;
                experience = 0;
            }            
        }
        else // at max level will make the shotsPerSecond increase slowly 
        {
            expFill.color = Color.magenta;
            if (experience < levels[level])
                stats.attacksPerSecond += 0.002f;
        }
        expSlider.value = experience / levels[level];
    }

    // Sets the coins amount
    public void SetCoins()
    {
        coinsText.text = "Coins: " + coins;
    }

    // returns the instantiated player object
    public GameObject GetPlayer()
    {
        return localPlayer;
    }

    // returns between 0 and 1 the percentage of health left
    private float CalculateHealthPercentage()
    {
        return health / stats.maxHealth;
    }

    private void CreateBlankSkillJson(SkillPath.Weapon weapon, SkillPath.Special special)
    {
        string skillsString = JsonUtility.ToJson(new SkillPath(weapon, special));
        File.WriteAllText(Path.Combine(Application.dataPath, "skills", "blank.json"), skillsString);
    }
}
