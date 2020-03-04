using UnityEngine;
using System.Collections;

public class SpecialAttack : MonoBehaviour
{

    // Update is called once per frame
    virtual protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && PlayerStats.playerStats.ability >= PlayerStats.playerStats.stats.abilityCost)
        {
            if (Special())
            {
                PlayerStats.playerStats.ability -= PlayerStats.playerStats.stats.abilityCost;
            }
        }
    }

    virtual protected bool Special()
    {
        return true;
    }
}
