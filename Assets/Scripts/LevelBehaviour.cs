using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelBehaviour : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject playerObject;
    public List<GameObject> enemyPrefabs;
    public int enemyCount = 20;
    public Transform rooms;
    public float tileSize = 10f;
    public bool[,] map = new bool[20, 20];
    public int maxStepCount = 100;
    private int currentStepCount = 0;
    private Vector2Int currentPosition = Vector2Int.zero;
    private List<TileComponent> tiles = new();

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        currentStepCount = maxStepCount;
        Vector2Int startPosition = new(Random.Range(0, map.GetLength(0) - 1), Random.Range(0, map.GetLength(1) - 1));
        currentPosition = startPosition;

        // drunkard walk
        while(currentStepCount > 0)
        {
            EDirection direction = (EDirection)Random.Range(0, 4);

            Debug.Log(direction);
            switch (direction)
            {
                case EDirection.North:
                    if (currentPosition.y != 0) currentPosition.y--;
                    break;
                case EDirection.East:
                    if (currentPosition.x < map.GetLength(0) - 1) currentPosition.x++;
                    break;
                case EDirection.South:
                    if (currentPosition.y < map.GetLength(1) - 1) currentPosition.y++;
                    break;
                case EDirection.West:
                    if (currentPosition.x != 0) currentPosition.x--;
                    break;
                default:
                    break;
            }

            if (!map[currentPosition.x, currentPosition.y])
            {
                map[currentPosition.x, currentPosition.y] = true;
                currentStepCount--;
            }
        }
        Debug.Log("Generation finished!");

        // spawn tiles
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (!map[x, y]) continue;

                GameObject tileObject = Instantiate(roomPrefab, new Vector3(x * tileSize, 0, y * tileSize), Quaternion.identity, rooms); // maybe parent is another object
                TileComponent tile = tileObject.GetComponent<TileComponent>();
                tiles.Add(tile);

                if (x < map.GetLength(0) - 1)
                {
                    if (map[x + 1, y])
                    {
                        int open = (int)tile.openings + (int)EOpenings.East;
                        tile.openings |= (EOpenings)open;
                    }
                }
                if (x > 0)
                {
                    if (map[x - 1, y])
                    {
                        int open = (int)tile.openings + (int)EOpenings.West;
                        tile.openings |= (EOpenings)open;
                    }
                }
                if (y < map.GetLength(1) - 1)
                {
                    if (map[x, y + 1])
                    {
                        int open = (int)tile.openings + (int)EOpenings.North;
                        tile.openings |= (EOpenings)open;
                    }
                }
                if (y > 0)
                {
                    if (map[x, y - 1])
                    {
                        int open = (int)tile.openings + (int)EOpenings.South;
                        tile.openings |= (EOpenings)open;
                    }
                }
            }
        }

        List<TileComponent> nonEnemyTiles = tiles.ToList();

        for (int i = 0; i < enemyCount; i++)
        {
            int tileIndex = Random.Range(0, nonEnemyTiles.Count - 1);
            nonEnemyTiles.RemoveAt(tileIndex);
        }

        foreach (TileComponent tile in tiles)
        {
            tile.walls[0].SetActive(!tile.openings.HasFlag(EOpenings.North));
            tile.walls[1].SetActive(!tile.openings.HasFlag(EOpenings.East));
            tile.walls[2].SetActive(!tile.openings.HasFlag(EOpenings.South));
            tile.walls[3].SetActive(!tile.openings.HasFlag(EOpenings.West));

            if (!nonEnemyTiles.Contains(tile))
            {
                GameObject enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                Instantiate(enemy, tile.transform.position + new Vector3(tileSize * 0.5f, 1, tileSize * 0.5f), Quaternion.identity);
            }
        }

        playerObject.transform.position = new Vector3(startPosition.x * tileSize, 1, startPosition.y * tileSize);
    }

    #region unused
    /*
    public EOpenings openings = (EOpenings)15;
    public int possibilities = 16;
    public bool visited { get; private set; }

    public bool CanPlace(EOpenings other)
    {
        switch (other)
        {
            case EOpenings.None:
                return false;
            case EOpenings.North:
                return openings.HasFlag(EOpenings.South);
            case EOpenings.East:
                return openings.HasFlag(EOpenings.West);
            case EOpenings.South:
                return openings.HasFlag(EOpenings.North);
            case EOpenings.West:
                return openings.HasFlag(EOpenings.South);
            default:
                return false;
        }
    }

    public void Collapse(bool first)
    {
        if (first)
        {
            int rand = Random.Range(1, possibilities - 1);

            openings = (EOpenings)rand;
            possibilities = 1;
            visited = true;
        }
    }

    public void UpdatePossibilities()
    {
        if (possibilities <= 1) return;

        for (int i = 1; i < 4; i++)
        {

        }
    }
    */
    #endregion
}

[System.Flags]
public enum EOpenings
{
    None = 0,
    North = 1,
    East = 2,
    South = 4,
    West = 8
}

public enum EDirection
{
    North,
    East,
    South,
    West
}
