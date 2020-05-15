using UnityEngine;
using System.Collections;

public class SpecialAttack : MonoBehaviour
{

    // Update is called once per frame
    virtual public void ManualUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && PlayerStats.playerStats.ability >= PlayerStats.playerStats.stats.abilityCost)
        {
            Debug.Log("ok");
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
