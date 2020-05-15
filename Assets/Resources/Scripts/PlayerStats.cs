using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats playerStats;

    public LevelChanger levelChanger;

    public GameObject playerPrefab;
    private GameObject localPlayer;
    public Transform playerPos;
    public Text healthText;
    public Slider healthSlider;
    public Text abilityText;
    public Slider abilitySlider;
    public Text levelText;
    public Slider expSlider;
    public Image expFill;
    public Text enemiesKilledText;
    public PauseMenu pauseMenu;

    public GameObject demonKeyImg, zombieKeyImg, ogreKeyImg;
    public bool demonKey, zombieKey, ogreKey;

    public SkillPath stats;

    public float health;
    public float ability;    

    public int enemiesKilled;

    public int level, path;
    public int[] levels = { 15, 50, 5000};
    public float experience;

    public float healthRegen;

    private float damageCooldown = 0;

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
        stats = SkillPath.skillPaths[0];

        health = stats.maxHealth;
        healthRegen = stats.healthRegenRate;
        ability = 0;
        level = 0;
        path = 0;
        SetHealthUI();
        SetAbilityUI();
        SetExpUI();
        stats.invulnerable = false;
        localPlayer = Instantiate(playerPrefab, Vector2.zero, Quaternion.identity);
        playerPos = localPlayer.transform;
        zombieKey = false;
        demonKey = false;
        ogreKey = false;
    }

    private void Update()
    {
        if (ability < stats.maxAbility)
            ability += stats.abilityChargeRate * Time.deltaTime;
        else
            ability = stats.maxAbility;
        SetAbilityUI();

        if (!stats.invulnerable)
        {
            health += healthRegen * Time.deltaTime;
            CheckDeath();
        }
        SetHealthUI();

        if (damageCooldown > 0)
        {
            damageCooldown -= Time.deltaTime;
        }
    }

    // damages the player
    public void DealDamage(float damage)
    {
        if (damageCooldown <= 0)
        {
            if (!stats.invulnerable)
            {
                health -= damage;
                CheckDeath();
                SetHealthUI();
                damageCooldown = 0.2f;
            }
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
            FindObjectOfType<AudioManager>().Play("GameOver");
            health = 0;
            levelChanger.FadeToLevel(2);     
        }
        else
            FindObjectOfType<AudioManager>().Play("PlayerHit");
    }

    // checks to see if the user won
    private void CheckWin()
    {
        if (demonKey && ogreKey && zombieKey)
        {
            FindObjectOfType<AudioManager>().Play("WinNoise");
            levelChanger.FadeToLevel(2);
        }
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
                SetLevelUI();
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
    public void SetLevelUI()
    {
        levelText.text = "LEVEL " + (level + 1);
    }

    // returns the instantiated player object
    public GameObject GetPlayerObject()
    {
        return localPlayer;
    }

    public Player GetPlayer()
    {
        return localPlayer.GetComponent<Player>();
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

    public void GiveDemonKey()
    {
        demonKeyImg.SetActive(true);
        demonKey = true;
        CheckWin();
    }

    public void GiveOgreKey()
    {
        ogreKeyImg.SetActive(true);
        ogreKey = true;
        CheckWin();
    }

    public void GiveZombieKey()
    {
        zombieKeyImg.SetActive(true);
        zombieKey = true;
        CheckWin();
    }
}
