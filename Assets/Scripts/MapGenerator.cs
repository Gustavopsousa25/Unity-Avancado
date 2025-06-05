using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int maxRoom;
    [SerializeField] private GameObject lobbyRoomPrefab, enemieRoomPrefab, bossRoomPrefab, doorPrefab, playerPrefab;

    [SerializeField] float roomSpacing = 2f;


    [SerializeField] List<Direction> allDirections = new List<Direction>();

    private List<GameObject> rooms = new List<GameObject>();
    private List<GameObject> doors = new List<GameObject>();
    private Vector2Int currentPos = Vector2Int.zero;

    List<Vector2Int> occupiedPosition = new List<Vector2Int>();

    enum Direction
    {
        north,
        south,
        west,
        east
    }
    private void Awake()
    {
        SpawnPlayer();
        StartGenerate();
    }

    void Update()
    {
         if (Input.GetKeyDown(KeyCode.R))
        {
            ClearMap();
            StartGenerate();
        }
    }

    void StartGenerate()
    {
        currentPos = Vector2Int.zero;
        GameObject firstRoom = Instantiate(lobbyRoomPrefab, Vector3.zero,Quaternion.identity);
        rooms.Add(firstRoom);
        occupiedPosition.Add(currentPos);
        GenerateMap();
        PlaceDoors();
    }

    private void GenerateMap()
    {
        for(int i= 1; i <= maxRoom; i++)
        {
            if (Random.value < 0.2f && occupiedPosition.Count > 1)
            {
                int parentRoom = Random.Range(0, occupiedPosition.Count);
                currentPos = occupiedPosition[parentRoom];
            }
            List<Direction> availableDirections = new List<Direction>(allDirections);
            bool canExpand = GenerateDirection(availableDirections);
            if (!canExpand)
            {
                i--; 
                continue;
            }
            if (i == maxRoom)
            {
                rooms.Add(Instantiate(bossRoomPrefab, new Vector3(currentPos.x, 0, currentPos.y) * roomSpacing, Quaternion.identity));
                occupiedPosition.Add(currentPos);
            }
            else
            {
                rooms.Add(Instantiate(enemieRoomPrefab, new Vector3(currentPos.x, 0, currentPos.y) * roomSpacing, Quaternion.identity));
                occupiedPosition.Add(currentPos);
            }
           
            
        }

    }

    private bool GenerateDirection(List<Direction> availableDirections)
    {
        if(availableDirections.Count == 0)
        {
            return false;
        }
        int index = Random.Range(0, availableDirections.Count);

        Direction direction = availableDirections[index];

        Vector2Int previousPos = currentPos;

        switch(direction)
        {
            case Direction.north:
                currentPos.y++;
                break;
            case Direction.south:
                currentPos.y--;
                break;
            case Direction.east:
                currentPos.x++;
                break;
            case Direction.west:
                currentPos.x--;
                break;
        }

        if(occupiedPosition.Contains(currentPos))
        {

            currentPos = previousPos;
            availableDirections.RemoveAt(index);
            return GenerateDirection(availableDirections);
        }
        return true;
    }
    private void SpawnPlayer()
    {
        if (playerPrefab != null)
        Instantiate(playerPrefab,new Vector3(0,1,0), Quaternion.identity);
    }
    private void InstantiateDoorInRoom(GameObject room, Direction doorDirection, float doorOffset)
    {
        Vector3 doorPos = room.transform.position;
        Quaternion doorRot = Quaternion.identity;

        switch (doorDirection)
        {
            case Direction.north:
                doorPos += new Vector3(0, 0, doorOffset);
                doorRot = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.south:
                doorPos += new Vector3(0, 0, -doorOffset);
                doorRot = Quaternion.Euler(0, 180, 0);
                break;
            case Direction.east:
                doorPos += new Vector3(doorOffset, 0, 0);
                doorRot = Quaternion.Euler(0, 90, 0);
                break;
            case Direction.west:
                doorPos += new Vector3(-doorOffset, 0, 0);
                doorRot = Quaternion.Euler(0, 270, 0);
                break;
        }
        RoomContentHolder holder = room.GetComponentInChildren<RoomContentHolder>();
        GameObject newDoor = Instantiate(doorPrefab, doorPos, doorRot, holder.transform);
        doors.Add(newDoor);
    }
    private void PlaceDoors()
    {
        float doorOffset = roomSpacing / 3f; 

        foreach (GameObject room in rooms)
        {
            Vector3 roomPos = room.transform.position;
            Vector2Int roomGridPos = new Vector2Int(Mathf.RoundToInt(roomPos.x / roomSpacing),Mathf.RoundToInt(roomPos.z / roomSpacing));

            // Check neighbors and instantiate doors
            if (occupiedPosition.Contains(roomGridPos + Vector2Int.up))
            {
                InstantiateDoorInRoom(room, Direction.north, doorOffset);
            }
            if (occupiedPosition.Contains(roomGridPos + Vector2Int.down)) 
            {
                InstantiateDoorInRoom(room, Direction.south, doorOffset);
            }
            if (occupiedPosition.Contains(roomGridPos + Vector2Int.right)) 
            {
                InstantiateDoorInRoom(room, Direction.east, doorOffset);
            }
            if (occupiedPosition.Contains(roomGridPos + Vector2Int.left)) 
            {
                InstantiateDoorInRoom(room, Direction.west, doorOffset);
            }
        }
    }
    private void ClearMap()
    {
        foreach (GameObject room in rooms)
        {
            if (room != null)
                Destroy(room);
        }
        rooms.Clear();

        foreach (GameObject door in doors)
        {
            if (door != null)
                Destroy(door);
        }
        doors.Clear();

        occupiedPosition.Clear();
        currentPos = Vector2Int.zero;
    }
}
