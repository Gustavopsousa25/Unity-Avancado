using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomContentHolder : MonoBehaviour
{
    [SerializeField] private Door doorPrefab;
    private MapGenerator mapGenerator;
    private Vector2Int roomGridPos;
    private List<Door> doors = new List<Door>();
    private EnemieSpawner Spawner;
    [SerializeField] private float doorDistance = 8f;

    Dictionary<Direction, Door> doorsDict = new Dictionary<Direction, Door>();
    private void Awake()
    {
        Spawner = GetComponent<EnemieSpawner>();
        Spawner.RoomClear += () => EnableDoor();
    }
    private void Start()
    {
        roomGridPos = new Vector2Int(Mathf.RoundToInt(transform.position.x / mapGenerator.RoomSpacing), Mathf.RoundToInt(transform.position.z / mapGenerator.RoomSpacing));
    }

    public void Init(MapGenerator generator)
    {
        mapGenerator = generator;
    }
    public void SetRoomPos(Vector2Int roomPos)
    {
        roomGridPos = roomPos;
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
                doorPos += new Vector3(0, 1, doorDistance);
                doorRot = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.south:
                doorPos += new Vector3(0, 1, -doorDistance);
                doorRot = Quaternion.Euler(0, 180, 0);
                break;
            case Direction.east:
                doorPos += new Vector3(doorDistance, 1, 0);
                doorRot = Quaternion.Euler(0, 90, 0);
                break;
            case Direction.west:
                doorPos += new Vector3(-doorDistance, 1, 0);
                doorRot = Quaternion.Euler(0, 270, 0);
                break;
        }

        Door newDoor = Instantiate(doorPrefab, doorPos, doorRot, transform);
        doorsDict.Add(doorDirection,newDoor);

        doors.Add(newDoor);
    }
    public void SetUpRoomDoors()
    {
        foreach (var door in doorsDict)
        {
            Direction dir = door.Key;
            Door newDoor = door.Value;

            Vector2Int offset = Vector2Int.zero;

            switch (dir)
            {
                case Direction.north:
                    offset.y++;
                    break;
                case Direction.south:
                    offset.y--;
                    break;
                case Direction.east:
                    offset.x++;
                    break;
                case Direction.west:
                    offset.x--;
                    break;
            }

            Direction oppoDir = OppositeDirection(dir);

            RoomContentHolder room = mapGenerator.GetRoomByGridPos(roomGridPos + offset);
            if(room.TryGetDoorByDirection(oppoDir, out Door doorDest))
            {
                newDoor.SetDoorDestination(doorDest.transform);
            }
        }
        if (gameObject.TryGetComponent(out EnemieSpawner enemieSpawner))
        {
            SubscribeToDoors();
        }
    }

    public bool TryGetDoorByDirection(Direction dir, out Door door)
    {
        door = null;

        if(doorsDict.TryGetValue(dir,out Door newDoor))
        {
            door = newDoor;
            return true;
        }

        return false;
    }
    private Direction OppositeDirection(Direction dir)
    {

        if(dir==Direction.north)
            return Direction.south;
        else if(dir==Direction.south)
            return Direction.north;
        else if(dir==Direction.east)
            return Direction.west;
        else
            return Direction.east;
    }
    private void SubscribeToDoors()
    {
        foreach (var door in doors)
        {
            door.OnPlayerTeleported += () => DisableDoor();
        }
    }
    private void DisableDoor()
    {
        foreach (var door in doors)
        {
            MeshRenderer doorMesh = door.GetComponent<MeshRenderer>();
            if (doorMesh != null)
            {
                doorMesh.material.color = Color.red;
            }
            door.OnCoolDown = true;
        }
    }
    public void EnableDoor()
    {
        foreach (var door in doors)
        {
            MeshRenderer doorMesh = door.GetComponent<MeshRenderer>();
            if (doorMesh != null)
            {
                doorMesh.material.color = Color.white;
            }
            door.OnCoolDown = false;
        }
    }
}



