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

    public string title;
    public string description;

    public int path;

    public enum Weapon { BOW, SWORD, STAFF };
    public Weapon weapon;
    public string weaponSprite;

    public enum Special { NONE, DASH, THROWSWORD, THROWDAGGERS, CLOAK, ICEBEAM, DANGERDASH, FIRESPIN, IMPS}
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
}
