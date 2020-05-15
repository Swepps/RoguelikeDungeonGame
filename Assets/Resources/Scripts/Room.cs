using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private int x, y;
    public bool[] entrances;
    public GameObject roomObject;
    private Grid<Room> roomGrid;
    public GameObject[] doors;
    public int enemiesRemaining;
    public int distanceFromStart;

    private bool playerInRoom = false;
    private bool doorsOpen = false;
    public bool enemiesSpawned = false;
    private List<GameObject> roomEnemies;

    private Pathfinding pathfindingArea;

    private void Awake()
    {
        enemiesRemaining = int.MaxValue;
    }

    private void Start()
    {
        roomGrid = GameObject.FindGameObjectWithTag("RoomSpawner").GetComponent<RoomSpawner>().roomGrid;
        roomGrid.GetXY(gameObject.transform.position, out x, out y);        
        pathfindingArea = new Pathfinding(18, 18, new Vector3(-9f, -9f) + transform.position);
        if (x == 5 && y == 5)
            OpenDoors();
    }

    private void Update()
    {
        if (playerInRoom)
        {
            if (roomGrid.GetGridObject(x, y + 1) != null)
            {
                if (!roomGrid.GetGridObject(x, y + 1).gameObject.activeInHierarchy)
                {
                    roomGrid.GetGridObject(x, y + 1).playerInRoom = false;
                    roomGrid.GetGridObject(x, y + 1).gameObject.SetActive(true);
                    if (!roomGrid.GetGridObject(x, y + 1).enemiesSpawned)
                        roomGrid.GetGridObject(x, y + 1).SpawnEnemies();
                }
            }
            if (roomGrid.GetGridObject(x, y - 1) != null)
            {
                if (!roomGrid.GetGridObject(x, y - 1).gameObject.activeInHierarchy)
                {
                    roomGrid.GetGridObject(x, y - 1).playerInRoom = false;
                    roomGrid.GetGridObject(x, y - 1).gameObject.SetActive(true);
                    if (!roomGrid.GetGridObject(x, y - 1).enemiesSpawned)
                        roomGrid.GetGridObject(x, y - 1).SpawnEnemies();
                }
            }
            if (roomGrid.GetGridObject(x + 1, y) != null)
            {
                if (!roomGrid.GetGridObject(x + 1, y).gameObject.activeInHierarchy)
                {
                    roomGrid.GetGridObject(x + 1, y).playerInRoom = false;
                    roomGrid.GetGridObject(x + 1, y).gameObject.SetActive(true);
                    if (!roomGrid.GetGridObject(x + 1, y).enemiesSpawned)
                        roomGrid.GetGridObject(x + 1, y).SpawnEnemies();
                }
            }
            if (roomGrid.GetGridObject(x - 1, y) != null)
            {
                if (!roomGrid.GetGridObject(x - 1, y).gameObject.activeInHierarchy)
                {
                    roomGrid.GetGridObject(x - 1, y).playerInRoom = false;
                    roomGrid.GetGridObject(x - 1, y).gameObject.SetActive(true);
                    if (!roomGrid.GetGridObject(x - 1, y).enemiesSpawned)
                        roomGrid.GetGridObject(x - 1, y).SpawnEnemies();
                }
            }
        }

        if (!doorsOpen)
            if (enemiesRemaining <= 0)
                OpenDoors();

        if (gameObject.activeInHierarchy && Vector3.Distance(gameObject.transform.position, PlayerStats.playerStats.playerPos.position) > 24.0f)
        {
            gameObject.SetActive(false);
        }

    }

    public void SpawnEnemies()
    {        
        if (!enemiesSpawned)
        {
            enemiesRemaining = 0;
            roomEnemies = EnemyCollections.Instance.GetRandomEnemyGroup(distanceFromStart);

            List<GameObject> tempEnemies = new List<GameObject>();

            foreach (GameObject e in roomEnemies)
            {
                enemiesRemaining++;
                tempEnemies.Add(Instantiate(e, Vector2.one * (transform.position + Vector3.one * Random.Range(-8f, 8f)), Quaternion.identity));
            }
            roomEnemies = tempEnemies;
            enemiesSpawned = true;
        }
        
    }

    public void OpenDoors()
    {
        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i])
            {
                switch (i)
                {
                    case 0:
                        if (roomGrid.GetGridObject(x, y + 1) != null)
                        {
                            if (roomGrid.GetGridObject(x, y + 1).entrances[2])
                            {
                                roomGrid.GetGridObject(x, y + 1).doors[2].SetActive(false);
                                doors[0].SetActive(false);
                            }
                        }
                        break;
                    case 1:
                        if (roomGrid.GetGridObject(x + 1, y) != null)
                        {
                            if (roomGrid.GetGridObject(x + 1, y).entrances[3])
                            {
                                roomGrid.GetGridObject(x + 1, y).doors[3].SetActive(false);
                                doors[1].SetActive(false);
                            }
                        }
                        break;
                    case 2:
                        if (roomGrid.GetGridObject(x, y - 1) != null)
                        {
                            if (roomGrid.GetGridObject(x, y - 1).entrances[0])
                            {
                                roomGrid.GetGridObject(x, y - 1).doors[0].SetActive(false);
                                doors[2].SetActive(false);
                            }
                        }
                        break;
                    case 3:
                        if (roomGrid.GetGridObject(x - 1, y) != null)
                        {
                            if (roomGrid.GetGridObject(x - 1, y).entrances[1])
                            {
                                roomGrid.GetGridObject(x - 1, y).doors[1].SetActive(false);
                                doors[3].SetActive(false);
                            }
                        }
                        break;
                }
            }
        }
        FindObjectOfType<AudioManager>().Play("DoorOpen");
        doorsOpen = true;
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public void SetX(int x)
    {
        this.x = x;
    }

    public void SetY(int y)
    {
        this.y = y;
    }
        
    public override string ToString()
    {
        return "X: " + x + " - Y: " + y + "\nDistance: " + distanceFromStart;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRoom = true;
        }
    }

    public Pathfinding GetPathfinding()
    {
        return pathfindingArea;
    }
}
