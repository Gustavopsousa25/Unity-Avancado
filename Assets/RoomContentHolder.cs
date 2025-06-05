using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomContentHolder : MonoBehaviour
{
    [SerializeField] private GameObject doorPrefab;
    //static RoomContentHolder prefab;
    private MapGenerator mapGenerator;
    private Vector2Int roomGridPos;
    private List<GameObject> doors = new List<GameObject>();
    private float doorDistance = 3f;

    /*private void OnEnable()
    {
        prefab = this;
    }*/
    private void Start()
    {
        mapGenerator = MapGenerator.Instance;
        roomGridPos = new Vector2Int(Mathf.RoundToInt(transform.position.x / mapGenerator.RoomSpacing), Mathf.RoundToInt(transform.position.z / mapGenerator.RoomSpacing));
    }

    public RoomContentHolder(Vector2Int myGrid, MapGenerator generator)
    {
        roomGridPos = myGrid;
        mapGenerator = generator;
    }

    public void PlaceDoors()
    {
        RoomContentHolder[] adjacentRooms = mapGenerator.GetAdjacentRooms(roomGridPos);

        foreach (var room in adjacentRooms)
        {
            Vector2Int direction = room.roomGridPos - roomGridPos;

            if (direction == Vector2Int.up)
                InstantiateDoor(Direction.north);
            else if (direction == Vector2Int.down)
                InstantiateDoor(Direction.south);
            else if (direction == Vector2Int.right)
                InstantiateDoor(Direction.east);
            else if (direction == Vector2Int.left)
                InstantiateDoor(Direction.west);
        }
    }

    private void InstantiateDoor(Direction doorDirection)
    {
        Vector3 doorPos = transform.position;
        Quaternion doorRot = Quaternion.identity;

        switch (doorDirection)
        {
            case Direction.north:
                doorPos += new Vector3(0, 0, doorDistance);
                doorRot = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.south:
                doorPos += new Vector3(0, 0, -doorDistance);
                doorRot = Quaternion.Euler(0, 180, 0);
                break;
            case Direction.east:
                doorPos += new Vector3(doorDistance, 0, 0);
                doorRot = Quaternion.Euler(0, 90, 0);
                break;
            case Direction.west:
                doorPos += new Vector3(-doorDistance, 0, 0);
                doorRot = Quaternion.Euler(0, 270, 0);
                break;
        }

        GameObject newDoor = Instantiate(doorPrefab, doorPos, doorRot, transform);
        doors.Add(newDoor);
    }
}

