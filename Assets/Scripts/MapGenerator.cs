using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public List<Room> rooms = new List<Room>();
    public Room startRoom;

    public Vector2Int MapSize;

    private Dictionary<Vector2Int, Room> roomMap = new Dictionary<Vector2Int, Room>();


    void Start()
    {
//        roomMap.Add(Vector2Int.zero, startRoom);
    }

    private bool AddRoooms()
    {
        Room lastRoom = null;
        for (int x = 0; x < MapSize.x; x++)
        {
            for (int y = 0; y < MapSize.y; y++)
            {
                if (x == 0 && y == 0)
                {
                    roomMap.Add(Vector2Int.zero, startRoom);
                    lastRoom = startRoom;
                }
                else
                {
                    Vector2Int targetPoint = new(x, y);

                    for (int i = 0; i < 4; i++)
                    {
                        Vector2Int current = lastRoom.openings[i];
                        switch (i)
                        {
                            case 0:
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                        }
                    }
                }
            }
        }
        return true;
    }


}
