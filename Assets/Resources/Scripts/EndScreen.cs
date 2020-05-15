using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public LevelChanger levelChanger;
    public Text title;

    private void Start()
    {
        if (PlayerStats.playerStats.demonKey && PlayerStats.playerStats.ogreKey && PlayerStats.playerStats.zombieKey)
            title.text = "WINNER!\nYou collected all the keys!";
        else
        {
            title.text = "GAME OVER!\nDemon Key - " + PlayerStats.playerStats.demonKey
                + "\nOgre Key - " + PlayerStats.playerStats.ogreKey
                + "\nZombie Key - " + PlayerStats.playerStats.zombieKey;
        }
    }

    public void StartGame()
    {
        levelChanger.FadeToLevel(1);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}
