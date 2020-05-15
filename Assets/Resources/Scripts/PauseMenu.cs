using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject levelUpUI;
    public GameObject button1, button2, button3;
    private SkillPath[] potentialSkills;
    private Text[] textObjects;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameIsPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        levelUpUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        button3.SetActive(true);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LevelUpWindow()
    {
        levelUpUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;        

        switch (PlayerStats.playerStats.level)
        {
            case 0:
                potentialSkills = new SkillPath[3];
                for (int i = 0; i < 3; i++)
                {
                    potentialSkills[i] = SkillPath.skillPaths[i + 1];
                }
                break;
            case 1:
                potentialSkills = new SkillPath[2];
                for (int i = 0; i < 2; i++)
                {
                    potentialSkills[i] = SkillPath.skillPaths[i + 4 + PlayerStats.playerStats.path];
                }
                break;
        }

        /*// getting the correct stats from the files and loading them in
        if (PlayerStats.playerStats.level == 1)
        {
            potentialSkills = new SkillPath[2];
            for (int i = 0; i < 2; i++)
            {
                skillsString = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "skills", (PlayerStats.playerStats.level + 2) + "-" + (i + PlayerStats.playerStats.path) + ".json"));
                potentialSkills[i] = JsonUtility.FromJson<SkillPath>(skillsString);
            }
        }
        else if (PlayerStats.playerStats.level == 0 || PlayerStats.playerStats.level == 2)
        {
            potentialSkills = new SkillPath[3];
            for (int i = 0; i < 3; i++)
            {
                skillsString = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "skills", (PlayerStats.playerStats.level + 2) + "-" + (i + PlayerStats.playerStats.path) + ".json"));
                potentialSkills[i] = JsonUtility.FromJson<SkillPath>(skillsString);
            }
        }
        else if (PlayerStats.playerStats.path == 0)
        {
            potentialSkills = new SkillPath[2];
            for (int i = 0; i < 2; i++)
            {
                skillsString = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "skills", (PlayerStats.playerStats.level + 2) + "-" + (i + PlayerStats.playerStats.path) + ".json"));
                potentialSkills[i] = JsonUtility.FromJson<SkillPath>(skillsString);
            }
        }
        else if (PlayerStats.playerStats.path == 5)
        {
            potentialSkills = new SkillPath[2];
            for (int i = -1; i < 1; i++)
            {
                skillsString = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "skills", (PlayerStats.playerStats.level + 2) + "-" + (i + PlayerStats.playerStats.path) + ".json"));
                potentialSkills[i + 1] = JsonUtility.FromJson<SkillPath>(skillsString);
            }
        }
        else
        {
            potentialSkills = new SkillPath[3];
            for (int i = -1; i < 2; i++)
            {
                skillsString = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "skills", (PlayerStats.playerStats.level + 2) + "-" + (i + PlayerStats.playerStats.path) + ".json"));
                potentialSkills[i + 1] = JsonUtility.FromJson<SkillPath>(skillsString);
            }
        }*/

        // sets the buttons to their correct settings
        if (potentialSkills.Length == 2)
        {
            button1.transform.localPosition = new Vector3(-180, -32, 0);
            textObjects = button1.GetComponentsInChildren<Text>();
            textObjects[0].text = potentialSkills[0].title;
            textObjects[2].text = potentialSkills[0].description;

            button2.transform.localPosition = new Vector3(180, -32, 0);
            textObjects = button2.GetComponentsInChildren<Text>();
            textObjects[0].text = potentialSkills[1].title;
            textObjects[2].text = potentialSkills[1].description;

            button3.SetActive(false);
        }
        else
        {
            button1.GetComponent<RectTransform>().anchoredPosition = new Vector2(144, -32);
            textObjects = button1.GetComponentsInChildren<Text>();
            textObjects[0].text = potentialSkills[0].title;
            textObjects[2].text = potentialSkills[0].description;

            button2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -32);
            textObjects = button2.GetComponentsInChildren<Text>();
            textObjects[0].text = potentialSkills[1].title;
            textObjects[2].text = potentialSkills[1].description;

            button3.SetActive(true);
            button3.GetComponent<RectTransform>().anchoredPosition = new Vector2(-144, -32);
            textObjects = button3.GetComponentsInChildren<Text>();
            textObjects[0].text = potentialSkills[2].title;
            textObjects[2].text = potentialSkills[2].description;
        }
    }

    public void LevelUp(int path)
    {
        PlayerStats.playerStats.stats = potentialSkills[path];
        PlayerStats.playerStats.path = PlayerStats.playerStats.stats.path;
        PlayerStats.playerStats.GetPlayerObject().GetComponent<Player>().UpdateStats();
        PlayerStats.playerStats.GetPlayerObject().GetComponentInChildren<Weapon>().UpdateStats();
        Resume();
    }
}
