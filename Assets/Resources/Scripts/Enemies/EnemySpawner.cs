using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public float spawnDelay;
    public int spawnAmount; // -1 means infinite

    private float x, y;
    private Vector3 spawnPos;
    private GameObject player;

    private void Start()
    {
        player = PlayerStats.playerStats.GetPlayer();
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        if (player != null) {
            while (spawnAmount != 0)
            {
                yield return new WaitForSeconds(spawnDelay);
                do
                {
                    x = Random.Range(-8f, 8f);
                    y = Random.Range(-8f, 8f);
                    spawnPos = transform.position;
                    spawnPos.x += x;
                    spawnPos.y += y;
                } while (Vector2.Distance(spawnPos, player.transform.position) < 1);

                Instantiate(enemies[0], spawnPos, Quaternion.identity);
                spawnAmount--;
            }
        }
    }
}
