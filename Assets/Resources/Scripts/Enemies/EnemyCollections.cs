using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollections : MonoBehaviour
{
    public static EnemyCollections Instance { get; private set; }

    public List<List<List<GameObject>>> enemyGroups;

    public GameObject mudSlime;
    public GameObject slime;

    public GameObject goblin;
    public GameObject bossOgre;

    public GameObject bossDemon;

    public GameObject bossZombie;

    private void Awake()
    {
        Instance = this;
        Instance.InitialiseEnemyGroups();
    }

    public List<GameObject> GetRandomEnemyGroup(int distance)
    {
        if (distance < Instance.enemyGroups.Count)
            return Instance.enemyGroups[distance][Random.Range(0, Instance.enemyGroups[distance].Count - 1)];
        else
            return Instance.enemyGroups[Instance.enemyGroups.Count - 1][Random.Range(0, Instance.enemyGroups[Instance.enemyGroups.Count - 1].Count - 1)];
    }

    public void InitialiseEnemyGroups()
    {
        enemyGroups = new List<List<List<GameObject>>>
        {
            // distance 0 options
            new List<List<GameObject>>
            {
                // option 1
                new List<GameObject>
                {
                    slime,
                },

                // option 2
                new List<GameObject>
                {
                    mudSlime,
                    mudSlime,
                },

                // option 3
                new List<GameObject>
                {
                    mudSlime,
                    mudSlime,
                    mudSlime,
                },
            },
            // distance 1 options
            new List<List<GameObject>>
            {
                // option 1
                new List<GameObject>
                {
                    mudSlime,
                    mudSlime,
                    goblin,
                    slime,                    
                },

                // option 2
                new List<GameObject>
                {
                    slime,
                    slime,
                    goblin,                    
                },

                // option 3
                new List<GameObject>
                {
                    goblin,
                    goblin,
                    mudSlime,
                    mudSlime,
                    mudSlime,
                    mudSlime,
                },
            },
            // distance 2 options
            new List<List<GameObject>>
            {
                // option 1
                new List<GameObject>
                {
                    bossDemon,
                    bossOgre,
                    bossZombie,
                },

                // option 2
                new List<GameObject>
                {
                    bossZombie,
                    bossOgre,
                    slime,
                    slime,
                },

                // option 3
                new List<GameObject>
                {
                    mudSlime,
                    mudSlime,
                    mudSlime,
                    mudSlime,
                    bossOgre,
                    bossZombie,
                },
            }
        };
    }
}
