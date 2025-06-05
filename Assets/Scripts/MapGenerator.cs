using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int maxRoom;
    [SerializeField] private GameObject defaultRoomPrefab, doorPrefab, playerPrefab;

    [SerializeField] float roomSpacing = 2f;


    [SerializeField] List<Direction> allDirections = new List<Direction>();
    List<Direction> availableDirection = new List<Direction>(); 

    List<GameObject> rooms = new List<GameObject>();
    private Vector2Int currentPos = Vector2Int.zero;

    List<Vector2Int> occupiedPosition = new List<Vector2Int>();

    enum Direction
    {
        north,
        south,
        west,
        east
    }
    // Start is called before the first frame update
    void Start()
    {
        StartGenerate();
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGenerate()
    {
        rooms.Add(Instantiate(defaultRoomPrefab, Vector3.zero,Quaternion.identity));
        
        GenerateMap();

    }

    private void GenerateMap()
    {
        for(int i= 1; i <= maxRoom; i++)
        {
            availableDirection = allDirections;
            GenerateDirection();
            rooms.Add(Instantiate(defaultRoomPrefab, new Vector3(currentPos.x, 0, currentPos.y) * roomSpacing, Quaternion.identity));
            occupiedPosition.Add(currentPos);
        }
       PlaceDoors();

    }

    private void GenerateDirection()
    {

        Direction direction = availableDirection[Random.Range(0, availableDirection.Count)];

        Vector2Int temp = currentPos;

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

            currentPos = temp;
            if (availableDirection.Count == 0) return;
            availableDirection.Remove(direction);
            GenerateDirection();
        }

    }
    private void SpawnPlayer()
    {
        if (playerPrefab != null)
        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    }
    private void InstantiateDoorInRoom(Vector3 roomPosition, Direction doorDirection, float doorOffset)
    {
        Vector3 doorPos = roomPosition;
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

        Instantiate(doorPrefab, doorPos, doorRot);
    }
    private void PlaceDoors()
    {
        float doorOffset = roomSpacing / 3f;  // half room spacing

        foreach (GameObject room in rooms)
        {
            Vector3 roomPos = room.transform.position;
            Vector2Int roomGridPos = new Vector2Int(
                Mathf.RoundToInt(roomPos.x / roomSpacing),
                Mathf.RoundToInt(roomPos.z / roomSpacing)
            );

            // Check neighbors and instantiate doors
            if (occupiedPosition.Contains(roomGridPos + Vector2Int.up)) // north
            {
                InstantiateDoorInRoom(roomPos, Direction.north, doorOffset);
            }
            if (occupiedPosition.Contains(roomGridPos + Vector2Int.down)) // south
            {
                InstantiateDoorInRoom(roomPos, Direction.south, doorOffset);
            }
            if (occupiedPosition.Contains(roomGridPos + Vector2Int.right)) // east
            {
                InstantiateDoorInRoom(roomPos, Direction.east, doorOffset);
            }
            if (occupiedPosition.Contains(roomGridPos + Vector2Int.left)) // west
            {
                InstantiateDoorInRoom(roomPos, Direction.west, doorOffset);
            }
        }
    }
}
