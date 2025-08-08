using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generates a grid of rooms and connects them with doors.
/// </summary>
public class GridSpawner : MonoBehaviour
{
    public int rows = 2;
    public int columns = 2;
    public float spacing = 20f;
    public float density = 0.05f; // Controls number of extra random connections

    public GameObject roomPrefab;
    public GameObject doorWayPrefab;
    public Transform origin;

    private RoomController[,] rooms;
    private HashSet<(int, int, int, int)> connections = new();

    /// <summary>
    /// Entry point to generate rooms and connect them.
    /// </summary>
    public void Generate()
    {
        SpawnGrid();
        GenerateConnections();
        ApplyConnections();
    }

    /// <summary>
    /// Spawns a grid of rooms at evenly spaced intervals.
    /// </summary>
    void SpawnGrid()
    {
        if (roomPrefab == null || origin == null)
        {
            Debug.LogError("Missing roomPrefab or origin!");
            return;
        }

        rooms = new RoomController[rows, columns];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector3 offset = new(x * spacing, 0f, y * spacing);
                Vector3 spawnPosition = origin.position + offset;
                GameObject roomGO = Instantiate(roomPrefab, spawnPosition, Quaternion.identity, transform);

                RoomController rc = roomGO.GetComponent<RoomController>();
                rc.doorWayPrefab = doorWayPrefab;

                rooms[y, x] = rc;
            }
        }
    }

    /// <summary>
    /// Uses DFS to connect all rooms with at least one path (ensures full accessibility).
    /// Adds random extra connections based on density.
    /// </summary>
    void GenerateConnections()
    {
        bool[,] visited = new bool[rows, columns];
        DFS(0, 0, visited); // Connect all rooms using DFS for basic path

        List<(int, int, int, int)> possibleEdges = GetAllPossibleEdges();
        Shuffle(possibleEdges);

        int maxExtraConnections = Mathf.RoundToInt((rows * columns) * density);
        int added = 0;

        // Add optional extra paths if they don’t already exist
        foreach (var (y1, x1, y2, x2) in possibleEdges)
        {
            if (connections.Contains((y1, x1, y2, x2)) || connections.Contains((y2, x2, y1, x1)))
                continue;

            int connCount1 = CountConnections(y1, x1);
            int connCount2 = CountConnections(y2, x2);

            if (connCount1 < 3 && connCount2 < 3)
            {
                connections.Add((y1, x1, y2, x2));
                added++;
                if (added >= maxExtraConnections)
                    break;
            }
        }
    }

    /// <summary>
    /// Depth-first search to connect grid into one pathable graph.
    /// </summary>
    void DFS(int y, int x, bool[,] visited)
    {
        visited[y, x] = true;
        List<(int dy, int dx)> directions = new() { (0, 1), (1, 0), (0, -1), (-1, 0) };
        Shuffle(directions); // Randomize pathing

        foreach (var (dy, dx) in directions)
        {
            int ny = y + dy;
            int nx = x + dx;

            if (ny >= 0 && ny < rows && nx >= 0 && nx < columns && !visited[ny, nx])
            {
                connections.Add((y, x, ny, nx));
                DFS(ny, nx, visited);
            }
        }
    }

    /// <summary>
    /// Returns all valid neighbor connections.
    /// </summary>
    List<(int, int, int, int)> GetAllPossibleEdges()
    {
        List<(int, int, int, int)> edges = new();

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (x < columns - 1) edges.Add((y, x, y, x + 1));
                if (y < rows - 1) edges.Add((y, x, y + 1, x));
            }
        }

        return edges;
    }

    /// <summary>
    /// Counts how many connections a room has.
    /// </summary>
    int CountConnections(int y, int x)
    {
        int count = 0;
        foreach (var (a1, b1, a2, b2) in connections)
        {
            if ((a1 == y && b1 == x) || (a2 == y && b2 == x))
                count++;
        }
        return count;
    }

    /// <summary>
    /// Applies stored connections by replacing walls with doorways.
    /// </summary>
    void ApplyConnections()
    {
        foreach (var (y1, x1, y2, x2) in connections)
        {
            RoomController roomA = rooms[y1, x1];
            RoomController roomB = rooms[y2, x2];

            if (y1 == y2)
            {
                if (x1 < x2)
                {
                    roomA.AddDoorway("East");
                    roomB.AddDoorway("West", false);
                }
                else
                {
                    roomA.AddDoorway("West");
                    roomB.AddDoorway("East", false);
                }
            }
            else if (x1 == x2)
            {
                if (y1 < y2)
                {
                    roomA.AddDoorway("North");
                    roomB.AddDoorway("South", false);
                }
                else
                {
                    roomA.AddDoorway("South");
                    roomB.AddDoorway("North", false);
                }
            }
        }
    }

    /// <summary>
    /// Shuffles a list in-place using Fisher-Yates algorithm.
    /// </summary>
    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }
}
