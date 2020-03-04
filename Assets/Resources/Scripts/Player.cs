using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement playerMovement;
    private GameObject bow, sword, staff;
    private SpecialAttack oldSpecial;

    void Start()
    {       
        playerMovement.rb.freezeRotation = true;
        playerMovement.lockMovement = false;
        bow = gameObject.transform.GetChild(0).gameObject;
        sword = gameObject.transform.GetChild(1).gameObject;
        staff = gameObject.transform.GetChild(2).gameObject;
        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayerStats.playerStats.stats.invulnerable = !PlayerStats.playerStats.stats.invulnerable;
            print(PlayerStats.playerStats.stats.invulnerable);
        }
    }

    public void UpdateStats()
    {
        playerMovement.maxSpeed = PlayerStats.playerStats.stats.moveSpeed;
        playerMovement.moveSpeed = playerMovement.maxSpeed;
        SetWeapon(PlayerStats.playerStats.stats.weapon);
        oldSpecial = gameObject.GetComponent<SpecialAttack>();
        Destroy(gameObject.GetComponent<SpecialAttack>());
        SetSpecial(PlayerStats.playerStats.stats.special);        
        playerMovement.animator.runtimeAnimatorController = Resources.Load("Animation/" + PlayerStats.playerStats.stats.character + "_movement_controller") as RuntimeAnimatorController;
        playerMovement.animator.SetFloat("moveSpeed", PlayerStats.playerStats.stats.moveSpeed / 3f);
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
                break;
            default:
                break;
        }
    }
}
