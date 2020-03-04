using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    private Animator bowAnimator;

    override public void UpdateStats()
    {
        base.UpdateStats();
        bowAnimator = GetComponent<Animator>();
        bowAnimator.SetFloat("shotsPerSecond", attacksPerSecond);
    }

    override protected void Update()
    {
        base.Update();
        if (Input.GetMouseButton(0) && shootReady)
        {
            shootReady = false;
            SetAnimatorVars(true);
        }
        if (!Input.GetMouseButton(0))
        {
            SetAnimatorVars(false);
            if (!shootReady)
                shootReady = true;
        }
    }

    public void ShootReady()
    {
        shootReady = true;
    }  

    private void SetAnimatorVars(bool isShooting)    {
        
        if (isShooting)
        {
            bowAnimator.SetBool("isShooting", true);
        }
        else
        {
            bowAnimator.SetBool("isShooting", false);
        }
    }

    private void SetAnimator()
    {
        switch (PlayerStats.playerStats.stats.weaponSprite)
        {
            case "basic_bow":
                bowAnimator.runtimeAnimatorController = Resources.Load("Animation/" + PlayerStats.playerStats.stats.weaponSprite) as RuntimeAnimatorController;
                break;
        }
    }
}
