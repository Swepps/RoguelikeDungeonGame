using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialiseGame : MonoBehaviour
{
    public static InitialiseGame game;
    public EnemyCollections enemyCollection;
    public GameObject spawner;

    private void Awake()
    {
        if (game != null)
        {
            Destroy(game);
        }
        else
        {
            game = this;
        }
        SkillPath.InitialiseSkillPaths();
    }

    public void Start()
    {
        // Instantiate(spawner, Vector2.zero, Quaternion.identity);
        
    }
}
