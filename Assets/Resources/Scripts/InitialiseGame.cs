using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialiseGame : MonoBehaviour
{
    public static InitialiseGame game;
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
    }

    public void Start()
    {
        Instantiate(spawner, Vector2.zero, Quaternion.identity);
    }
}
