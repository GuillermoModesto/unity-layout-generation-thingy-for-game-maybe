using UnityEngine;

/// <summary>
/// Controls individual room logic, such as placing doorways.
/// </summary>
public class RoomController : MonoBehaviour
{
    public GameObject floor;
    public GameObject northWall;
    public GameObject southWall;
    public GameObject eastWall;
    public GameObject westWall;

    public GameObject doorWayPrefab;
    public GameObject doorWayTriggerPrefab;

    [Range(0f, 1f)]
    public float triggerChance = 0.5f; // Chance of doorway having a trigger

    /// <summary>
    /// Replaces a wall with a doorway (and optionally a trigger).
    /// </summary>
    /// <param name="wall">Wall direction: North, South, East, West</param>
    /// <param name="spawnTrigger">Whether to spawn a trigger</param>
    public void AddDoorway(string wall, bool spawnTrigger = true)
    {
        GameObject wallToRemove = null;
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        // Determine which wall to replace and set proper position/rotation
        switch (wall)
        {
            case "North":
                wallToRemove = northWall;
                position = new Vector3(wallToRemove.transform.position.x, 0, wallToRemove.transform.position.z);
                rotation = Quaternion.Euler(0, 0, 0);
                break;
            case "South":
                wallToRemove = southWall;
                position = new Vector3(wallToRemove.transform.position.x, 0, wallToRemove.transform.position.z);
                rotation = Quaternion.Euler(0, 180, 0);
                break;
            case "East":
                wallToRemove = eastWall;
                position = new Vector3(wallToRemove.transform.position.x, 0, wallToRemove.transform.position.z);
                rotation = Quaternion.Euler(0, 90, 0);
                break;
            case "West":
                wallToRemove = westWall;
                position = new Vector3(wallToRemove.transform.position.x, 0, wallToRemove.transform.position.z);
                rotation = Quaternion.Euler(0, -90, 0);
                break;
        }

        if (wallToRemove != null)
        {
            Destroy(wallToRemove);
            Instantiate(doorWayPrefab, position, rotation, this.transform);

            // Randomly add a doorway trigger if enabled
            if (spawnTrigger && doorWayTriggerPrefab != null && Random.value < triggerChance)
            {
                Instantiate(doorWayTriggerPrefab, new Vector3(position.x, 2.5f, position.z), rotation, this.transform);
            }
        }
    }
}
