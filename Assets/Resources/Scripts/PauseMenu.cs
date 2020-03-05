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
    public Button button1, button2, button3;
    private GameObject button3Object;
    private SkillPath[] potentialSkills;
    private Text[] textObjects;

    private void Start()
    {
        button3Object = GameObject.Find("Path Three Button");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameIsPaused)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void Resume()
    {
        levelUpUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        button3Object.SetActive(true);
    }

    public void Pause()
    {
        levelUpUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LevelUpWindow()
    {
        levelUpUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;        
        //string skillsString;
        button3Object = GameObject.Find("Path Three Button");

        switch (PlayerStats.playerStats.level)
        {
            case 0:
                potentialSkills = new SkillPath[3];
                for (int i = 0; i < 3; i++)
                {
                    potentialSkills[i] = SkillPath.skillPaths[i + 1];
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

            button3Object.SetActive(false);
        }
        else
        {
            button1.transform.localPosition = new Vector3(-330, -32, 0);
            textObjects = button1.GetComponentsInChildren<Text>();
            textObjects[0].text = potentialSkills[0].title;
            textObjects[2].text = potentialSkills[0].description;

            button2.transform.localPosition = new Vector3(0, -32, 0);
            textObjects = button2.GetComponentsInChildren<Text>();
            textObjects[0].text = potentialSkills[1].title;
            textObjects[2].text = potentialSkills[1].description;

            button3Object.SetActive(true);
            button3.transform.localPosition = new Vector3(330, -32, 0);
            textObjects = button3.GetComponentsInChildren<Text>();
            textObjects[0].text = potentialSkills[2].title;
            textObjects[2].text = potentialSkills[2].description;
        }
    }

    public void LevelUp(int path)
    {
        PlayerStats.playerStats.stats = potentialSkills[path];
        PlayerStats.playerStats.path = PlayerStats.playerStats.stats.path;
        PlayerStats.playerStats.GetPlayer().GetComponent<Player>().UpdateStats();
        PlayerStats.playerStats.GetPlayer().GetComponentInChildren<Weapon>().UpdateStats();
        Resume();
    }
}
