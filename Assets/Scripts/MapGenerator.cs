using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    static MapGenerator instance;
    [SerializeField] private int maxRoom;
    [SerializeField] private GameObject lobbyRoomPrefab, enemieRoomPrefab, bossRoomPrefab, playerPrefab;
    [SerializeField] float roomSpacing = 2f;
    [SerializeField] List<Direction> allDirections = new List<Direction>();

    private List<GameObject> rooms = new List<GameObject>();
    private Vector2Int currentPos = Vector2Int.zero;

    List<Vector2Int> occupiedPosition = new List<Vector2Int>();

    public float RoomSpacing { get => roomSpacing; set => roomSpacing = value; }
    public static MapGenerator Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        instance = this;
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
        GameObject firstRoom = Instantiate(lobbyRoomPrefab, Vector3.zero, Quaternion.identity);
        rooms.Add(firstRoom);
        occupiedPosition.Add(currentPos);
        GenerateMap();
    }

    public RoomContentHolder GetRoomByGridPos(Vector2Int targetGridPos)
    {
        int index = occupiedPosition.IndexOf(targetGridPos);

        GameObject room = rooms[index];
        return room.GetComponentInChildren<RoomContentHolder>();
    }

    private async void GenerateMap()
    {
        for (int i = 1; i <= maxRoom; i++)
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
                rooms.Add(Instantiate(bossRoomPrefab, new Vector3(currentPos.x, 0, currentPos.y) * RoomSpacing, Quaternion.identity));

                occupiedPosition.Add(currentPos);
            }
            else
            {
                rooms.Add(Instantiate(enemieRoomPrefab, new Vector3(currentPos.x, 0, currentPos.y) * RoomSpacing, Quaternion.identity));
                occupiedPosition.Add(currentPos);
            }
            RoomContentHolder roomContent = rooms[rooms.Count-1].GetComponentInChildren<RoomContentHolder>();
            roomContent.SetRoomPos(currentPos);
        }

        rooms.ForEach(room => {
                RoomContentHolder roomContent = room.GetComponentInChildren<RoomContentHolder>();
                roomContent.Init(this);
                roomContent.PlaceDoors();
            });

        StartCoroutine(MyRoutine());

    }

    IEnumerator MyRoutine()
    {

        yield return new WaitForSeconds(1f); // ← One-liner delay
        rooms.ForEach(room => room.GetComponentInChildren<RoomContentHolder>().SetUpRoomDoors());
    }

    private bool GenerateDirection(List<Direction> availableDirections)
    {
        if (availableDirections.Count == 0)
        {
            return false;
        }
        int index = Random.Range(0, availableDirections.Count);

        Direction direction = availableDirections[index];

        Vector2Int previousPos = currentPos;

        switch (direction)
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

        if (occupiedPosition.Contains(currentPos))
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
            Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
    }
    public RoomContentHolder[] GetAdjacentRooms(Vector2Int roomGridPos)
    {
        List<RoomContentHolder> adjacentRooms = new List<RoomContentHolder>();

        foreach (var room in rooms)
        {
            if (room == null)
            {
                Debug.LogWarning("Found null room in rooms list.");
                continue;
            }

            Vector2Int roomPos = new Vector2Int(Mathf.RoundToInt(room.transform.position.x / RoomSpacing),Mathf.RoundToInt(room.transform.position.z / RoomSpacing));

            if (roomPos == roomGridPos + Vector2Int.up ||roomPos == roomGridPos + Vector2Int.down ||roomPos == roomGridPos + Vector2Int.left ||roomPos == roomGridPos + Vector2Int.right)
            {
                RoomContentHolder holder = room.GetComponentInChildren<RoomContentHolder>();
                if (holder != null)
                {
                    adjacentRooms.Add(holder);
                }
                else
                {
                    Debug.LogWarning($"Room at {room.transform.position} does not have a RoomContentHolder.");
                }
            }
        }
        return adjacentRooms.ToArray();
    }

    private void ClearMap()
    {
        foreach (GameObject room in rooms)
        {
            if (room != null)
                Destroy(room);
        }
        rooms.Clear();
        occupiedPosition.Clear();
        currentPos = Vector2Int.zero;
    }
}

public enum Direction
{
    north,
    south,
    east,
    west
}