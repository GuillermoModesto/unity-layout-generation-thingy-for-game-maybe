using UnityEngine;

public enum DoorDirection { North, South, East, West }

public class RoomDoorway : MonoBehaviour
{
    public DoorDirection direction;
    public bool isConnected = false;

    public GameObject triggerDoorwayPrefab;
    public GameObject nonTriggerDoorwayPrefab;
}
