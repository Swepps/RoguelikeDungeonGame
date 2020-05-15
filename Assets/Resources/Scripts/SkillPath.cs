using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPath
{   
    public SkillPath (Weapon weapon, Special special)
    {
        this.weapon = weapon;
        this.special = special;
    }

    public SkillPath()
    {

    }

    public string title;
    public string description;

    public int path;

    public enum Weapon { BOW, SWORD, STAFF };
    public Weapon weapon;
    public string weaponSprite;

    public enum Special { NONE, DASH, THROWSWORD, THROWDAGGERS, CLOAK, ICEBEAM, DANGERDASH, FIRESPIN, IMPS, FIREBALL}
    public Special special;

    public string character;

    public float maxHealth;
    public float healthRegenRate;

    public float maxAbility;
    public float abilityChargeRate;
    public float abilityCost;

    public float attacksPerSecond;
    public float projectileForce;
    public float projectileRange;
    public float damageMin, damageMax;
    public float knockback;
    public int shotSpread;
    public bool parallel;

    public float moveSpeed;   

    public bool invulnerable;

    public static SkillPath[] skillPaths;

    // hard coding in the values in order to work with WebGL
    public static void InitialiseSkillPaths()
    {
        skillPaths = new SkillPath[26];

        skillPaths[0] = new SkillPath
        {
            title = "",
            description = "",
            path = 0,
            weapon = Weapon.SWORD,
            weaponSprite = "rusty_sword",
            special = Special.NONE,
            character = "man",
            maxHealth = 50.0f,
            healthRegenRate = 0,
            maxAbility = 50.0f,
            abilityChargeRate = 0,
            abilityCost = 20.0f,
            attacksPerSecond = 2.0f,
            projectileForce = 10.0f,
            projectileRange = 6.0f,
            damageMin = 5.0f,
            damageMax = 10.0f,
            knockback = 4.0f,
            shotSpread = 0,
            parallel = false,
            moveSpeed = 3.0f,
            invulnerable = false
        };

        skillPaths[1] = new SkillPath
        {
            title = "Novice Archer",
            description = "- Shoots a bow and arrow\n- Quicker Move Speed",
            path = 0,
            weapon = Weapon.BOW,
            weaponSprite = "basic_bow",
            special = Special.NONE,
            character = "elf",
            maxHealth = 50.0f,
            healthRegenRate = 0,
            maxAbility = 50.0f,
            abilityChargeRate = 0,
            abilityCost = 20.0f,
            attacksPerSecond = 1.5f,
            projectileForce = 10.0f,
            projectileRange = 6.0f,
            damageMin = 5.0f,
            damageMax = 10.0f,
            knockback = 0.1f,
            shotSpread = 0,
            parallel = false,
            moveSpeed = 3.5f,
            invulnerable = false
        };

        skillPaths[2] = new SkillPath
        {
            title = "Novice Knight",
            description = "- Swings a sword for lots of damage\n- Has a higher max health",
            path = 1,
            weapon = Weapon.SWORD,
            weaponSprite = "regular_sword",
            special = Special.NONE,
            character = "knight",
            maxHealth = 75.0f,
            healthRegenRate = 0f,
            maxAbility = 50.0f,
            abilityChargeRate = 0,
            abilityCost = 20.0f,
            attacksPerSecond = 1.2f,
            projectileForce = 10.0f,
            projectileRange = 6.0f,
            damageMin = 10.0f,
            damageMax = 15.0f,
            knockback = 8.0f,
            shotSpread = 0,
            parallel = false,
            moveSpeed = 3.0f,
            invulnerable = false
        };

        skillPaths[3] = new SkillPath
        {
            title = "Novice Mage",
            description = "- Shoots fireballs from his staff",
            path = 2,
            weapon = Weapon.STAFF,
            weaponSprite = "red_magic_staff",
            special = Special.NONE,
            character = "wizard",
            maxHealth = 50.0f,
            healthRegenRate = 0,
            maxAbility = 50.0f,
            abilityChargeRate = 0,
            abilityCost = 20.0f,
            attacksPerSecond = 0.8f,
            projectileForce = 7.0f,
            projectileRange = 6.0f,
            damageMin = 8.0f,
            damageMax = 12.0f,
            knockback = 1.0f,
            shotSpread = 0,
            parallel = false,
            moveSpeed = 3.0f,
            invulnerable = false
        };

        skillPaths[4] = new SkillPath
        {
            title = "Apprentice Archer",
            description = "- Shoots three arrows at once\n- Can dash with SPACE",
            path = 0,
            weapon = Weapon.BOW,
            weaponSprite = "basic_bow",
            special = Special.DASH,
            character = "elf",
            maxHealth = 50.0f,
            healthRegenRate = 0,
            maxAbility = 50.0f,
            abilityChargeRate = 10f,
            abilityCost = 30.0f,
            attacksPerSecond = 1.5f,
            projectileForce = 10.0f,
            projectileRange = 6.0f,
            damageMin = 4.0f,
            damageMax = 7.0f,
            knockback = 0.1f,
            shotSpread = 3,
            parallel = false,
            moveSpeed = 3.5f,
            invulnerable = false
        };

        skillPaths[5] = new SkillPath
        {
            title = "Rookie Skirmisher",
            description = "- Swings a hatchet\n- Can throw his hatchet with SPACE\n - Runs faster",
            path = 1,
            weapon = Weapon.SWORD,
            weaponSprite = "axe",
            special = Special.THROWSWORD,
            character = "elf",
            maxHealth = 60.0f,
            healthRegenRate = 0f,
            maxAbility = 50.0f,
            abilityChargeRate = 8f,
            abilityCost = 50.0f,
            attacksPerSecond = 2.5f,
            projectileForce = 10.0f,
            projectileRange = 6.0f,
            damageMin = 12.0f,
            damageMax = 18.0f,
            knockback = 5.0f,
            shotSpread = 0,
            parallel = false,
            moveSpeed = 4.0f,
            invulnerable = false
        };

        skillPaths[6] = new SkillPath
        {
            title = "Magically Inclined Knight",
            description = "- Can throw a fireball with SPACE",
            path = 2,
            weapon = Weapon.SWORD,
            weaponSprite = "regular_sword",
            special = Special.FIREBALL,
            character = "knight",
            maxHealth = 75.0f,
            healthRegenRate = 0f,
            maxAbility = 50.0f,
            abilityChargeRate = 5f,
            abilityCost = 40.0f,
            attacksPerSecond = 1.5f,
            projectileForce = 10.0f,
            projectileRange = 6.0f,
            damageMin = 12.0f,
            damageMax = 17.0f,
            knockback = 10.0f,
            shotSpread = 0,
            parallel = false,
            moveSpeed = 3.5f,
            invulnerable = false
        };

        skillPaths[7] = new SkillPath
        {
            title = "Apprentice Mage",
            description = "- Shoots fireballs from a much further distance\n -Shoots a lot faster",
            path = 3,
            weapon = Weapon.STAFF,
            weaponSprite = "red_magic_staff",
            special = Special.NONE,
            character = "wizard",
            maxHealth = 50.0f,
            healthRegenRate = 0,
            maxAbility = 50.0f,
            abilityChargeRate = 0,
            abilityCost = 20.0f,
            attacksPerSecond = 2.0f,
            projectileForce = 10.0f,
            projectileRange = 12.0f,
            damageMin = 8.0f,
            damageMax = 12.0f,
            knockback = 1.0f,
            shotSpread = 0,
            parallel = false,
            moveSpeed = 3.5f,
            invulnerable = false
        };
    }
}
