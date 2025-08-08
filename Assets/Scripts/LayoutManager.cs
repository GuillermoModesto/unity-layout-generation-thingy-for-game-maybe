using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages multiple grid-based room layouts and swaps between them.
/// </summary>
public class LayoutManager : MonoBehaviour
{
    public int numberOfLayouts = 3;
    public GameObject roomPrefab;
    public GameObject doorWayPrefab;
    public Transform origin;
    public float spacing = 20f;
    public int rows = 3;
    public int columns = 3;

    private List<GameObject> layouts = new();
    private int currentLayoutIndex = 0;

    void Start()
    {
        GenerateLayouts();
        ShowLayout(0);
    }

    /// <summary>
    /// Switch to the next layout in the list.
    /// </summary>
    public void NextLayout()
    {
        layouts[currentLayoutIndex].SetActive(false);
        currentLayoutIndex = (currentLayoutIndex + 1) % layouts.Count;
        ShowLayout(currentLayoutIndex);
    }

    /// <summary>
    /// Creates multiple layouts using GridSpawner.
    /// </summary>
    void GenerateLayouts()
    {
        for (int i = 0; i < numberOfLayouts; i++)
        {
            GameObject layoutParent = new GameObject($"Layout_{i}");
            layoutParent.transform.parent = this.transform;

            // Add and configure the grid spawner
            GridSpawner spawner = layoutParent.AddComponent<GridSpawner>();
            spawner.rows = rows;
            spawner.columns = columns;
            spawner.spacing = spacing;
            spawner.roomPrefab = roomPrefab;
            spawner.doorWayPrefab = doorWayPrefab;
            spawner.origin = origin;

            spawner.Generate(); // Generate the grid and connect rooms

            layoutParent.SetActive(false); // Hide initially
            layouts.Add(layoutParent); // Add to list
        }
    }

    /// <summary>
    /// Activates a layout by index.
    /// </summary>
    void ShowLayout(int index)
    {
        layouts[index].SetActive(true);
    }
}
