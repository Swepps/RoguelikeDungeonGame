using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public Grid<Room> roomGrid;

    public Room startRoom;
    private List<Room> openList;

    private RoomTemplates templates;

    private int numRooms;

    private void Start()
    {
        templates = gameObject.GetComponent<RoomTemplates>();

        startRoom.SetX(5);
        startRoom.SetY(5);
        startRoom.distanceFromStart = 0;
        roomGrid = new Grid<Room>(11, 11, 24.0f, new Vector3(-132f, -132f));
        roomGrid.SetGridObject(5, 5, startRoom);

        openList = new List<Room>() { startRoom };

        numRooms = 1;

        int validEntrances = 0;

        while (openList.Count > 0)
        {
            validEntrances = 0;
            Room newRoom;
            Room current = openList[openList.Count - 1];
            int currentCellIndex = openList.Count - 1;

            // north entrance
            if (current.entrances[0])
            {
                if (current.GetY() + 1 < roomGrid.GetHeight() && roomGrid.GetGridObject(current.GetX(), current.GetY() + 1) == null)
                {
                    newRoom = SpawnValidRoom(current.GetX(), current.GetY() + 1, 0);
                    newRoom.SetX(current.GetX());
                    newRoom.SetY(current.GetY() + 1);
                    newRoom.distanceFromStart = current.distanceFromStart + 1;
                    openList.Add(newRoom);
                    roomGrid.SetGridObject(current.GetX(), current.GetY() + 1, newRoom);
                    validEntrances++;
                }
            }
            // east entrance
            if (current.entrances[1])
            {
                if (current.GetX() + 1 < roomGrid.GetWidth() && roomGrid.GetGridObject(current.GetX() + 1, current.GetY()) == null)
                {
                    newRoom = SpawnValidRoom(current.GetX() + 1, current.GetY(), 1);
                    newRoom.SetX(current.GetX() + 1);
                    newRoom.SetY(current.GetY());
                    newRoom.distanceFromStart = current.distanceFromStart + 1;
                    openList.Add(newRoom);
                    roomGrid.SetGridObject(current.GetX() + 1, current.GetY(), newRoom);
                    validEntrances++;
                }
            }
            // south entrance
            if (current.entrances[2])
            {
                if (current.GetY() - 1 >= 0 && roomGrid.GetGridObject(current.GetX(), current.GetY() - 1) == null)
                {
                    newRoom = SpawnValidRoom(current.GetX(), current.GetY() - 1, 2);
                    newRoom.SetX(current.GetX());
                    newRoom.SetY(current.GetY() - 1);
                    newRoom.distanceFromStart = current.distanceFromStart + 1;
                    openList.Add(newRoom);
                    roomGrid.SetGridObject(current.GetX(), current.GetY() - 1, newRoom);
                    validEntrances++;
                }
            }
            // west entrance
            if (current.entrances[3])
            {
                if (current.GetX() - 1 >= 0 && roomGrid.GetGridObject(current.GetX() - 1, current.GetY()) == null)
                {
                    newRoom = SpawnValidRoom(current.GetX() - 1, current.GetY(), 3);
                    newRoom.SetX(current.GetX() - 1);
                    newRoom.SetY(current.GetY());
                    newRoom.distanceFromStart = current.distanceFromStart + 1;
                    openList.Add(newRoom);
                    roomGrid.SetGridObject(current.GetX() - 1, current.GetY(), newRoom);
                    validEntrances++;
                }
            }

            if (validEntrances == 0)
                openList.RemoveAt(currentCellIndex);
        }

        for (int x = 0; x < roomGrid.GetWidth(); x++)
        {
            for (int y = 0; y < roomGrid.GetHeight(); y++)
            {
                if (x != 5 || y != 5)
                {
                    if (roomGrid.GetGridObject(x, y) != null)
                        roomGrid.GetGridObject(x, y).gameObject.SetActive(false);
                }
            }
        }
    }

    private Room SpawnValidRoom(int x, int y, int dir)
    {
        int validRoomCount = 0;

        Room roomClone;

        if (numRooms < 9)
        {
            roomClone = Instantiate(templates.bottomRooms[templates.bottomRooms.Length - 1], roomGrid.GetWorldPosition(x, y) + Vector3.one * (roomGrid.GetCellSize()/2), Quaternion.identity).GetComponent<Room>();
            roomClone.roomObject = templates.bottomRooms[templates.bottomRooms.Length - 1].roomObject;
            numRooms++;

            return roomClone;
        }        

        Room tempRoom = null;

        while (validRoomCount < 4)
        {
            switch (dir)
            {
                case 0:
                    tempRoom = templates.bottomRooms[Random.Range(0, templates.bottomRooms.Length)];
                    break;
                case 1:
                    tempRoom = templates.leftRooms[Random.Range(0, templates.leftRooms.Length)];
                    break;
                case 2:
                    tempRoom = templates.topRooms[Random.Range(0, templates.topRooms.Length)];
                    break;
                case 3:
                    tempRoom = templates.rightRooms[Random.Range(0, templates.rightRooms.Length)];
                    break;
            }            
            validRoomCount = 0;

            for (int i = 0; i < tempRoom.entrances.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (!tempRoom.entrances[i] || Mathf.Abs(dir - i) == 2)
                            validRoomCount++;
                        else if (y + 1 < roomGrid.GetHeight())
                        {
                            if (roomGrid.GetGridObject(x, y + 1) == null || roomGrid.GetGridObject(x, y + 1).entrances[2])
                                validRoomCount++;
                        }
                        break;
                    case 1:
                        if (!tempRoom.entrances[i] || Mathf.Abs(dir - i) == 2)
                            validRoomCount++;
                        else if (x + 1 < roomGrid.GetWidth())
                        {
                            if (roomGrid.GetGridObject(x + 1, y) == null || roomGrid.GetGridObject(x + 1, y).entrances[3])
                                validRoomCount++;
                        }
                        break;
                    case 2:
                        if (!tempRoom.entrances[i] || Mathf.Abs(dir - i) == 2)
                            validRoomCount++;
                        else if (y - 1 >= 0)
                        {
                            if (roomGrid.GetGridObject(x, y - 1) == null || roomGrid.GetGridObject(x, y - 1).entrances[0])
                                validRoomCount++;
                        }
                        break;
                    case 3:
                        if (!tempRoom.entrances[i] || Mathf.Abs(dir - i) == 2)
                            validRoomCount++;
                        else if (x - 1 >= 0)
                        {
                            if (roomGrid.GetGridObject(x - 1, y) == null || roomGrid.GetGridObject(x - 1, y).entrances[1])
                                validRoomCount++;
                        }
                        break;
                }
            }
        }

        roomClone = Instantiate(tempRoom.roomObject, roomGrid.GetWorldPosition(x, y) + Vector3.one * (roomGrid.GetCellSize() / 2), Quaternion.identity).GetComponent<Room>();
        roomClone.roomObject = tempRoom.roomObject;
        numRooms++;

        return roomClone;
    }
}
