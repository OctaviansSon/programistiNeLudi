using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public int roomCount = 8;
    public float spacing = 0f; // 0 — вычислить автоматически

    // map grid -> room
    public Dictionary<Vector2Int, DungeonRoom> rooms = new();

    Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    void Awake()
    {
        // вычислим spacing если не задано
        if (spacing <= 0f && roomPrefab != null)
        {
            GameObject tmp = Instantiate(roomPrefab);
            Renderer r = tmp.GetComponentInChildren<Renderer>();
            if (r != null)
            {
                Vector3 size = r.bounds.size;
                spacing = Mathf.Max(size.x, size.y);
            }
            else spacing = 14f; // запасной вариант
            Destroy(tmp);
        }

        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        if (roomPrefab == null)
        {
            Debug.LogError("DungeonGenerator: roomPrefab is null!");
            return;
        }

        rooms.Clear();

        // стартовая комната в (0,0)
        Vector2Int start = Vector2Int.zero;
        CreateRoom(start, true, false);

        List<Vector2Int> frontier = new List<Vector2Int>();
        AddFrontierNeighbors(start, frontier);

        for (int i = 1; i < roomCount; i++)
        {
            if (frontier.Count == 0)
            {
                // если фронтир пуст — расширяем от любой существующей комнаты
                List<Vector2Int> existing = new List<Vector2Int>(rooms.Keys);
                Vector2Int pick = existing[Random.Range(0, existing.Count)];
                AddFrontierNeighbors(pick, frontier);
                if (frontier.Count == 0)
                    break;
            }

            int idx = Random.Range(0, frontier.Count);
            Vector2Int pos = frontier[idx];
            frontier.RemoveAt(idx);

            CreateRoom(pos, false, i == roomCount - 1);

            // соединяем с уже существующими соседями
            foreach (var dir in directions)
            {
                Vector2Int neigh = pos + dir;
                if (rooms.ContainsKey(neigh))
                {
                    ConnectRooms(pos, neigh);
                }
            }

            AddFrontierNeighbors(pos, frontier);
        }
    }

    void AddFrontierNeighbors(Vector2Int center, List<Vector2Int> frontier)
    {
        foreach (var d in directions)
        {
            Vector2Int p = center + d;
            if (!rooms.ContainsKey(p) && !frontier.Contains(p))
                frontier.Add(p);
        }
    }

    void CreateRoom(Vector2Int pos, bool start, bool boss)
    {
        Vector3 worldPos = new Vector3(pos.x * spacing, pos.y * spacing, 0f);
        GameObject r = Instantiate(roomPrefab, worldPos, Quaternion.identity, transform);
        DungeonRoom room = r.GetComponent<DungeonRoom>();

        if (room == null)
        {
            Debug.LogError("roomPrefab missing DungeonRoom component!");
            Destroy(r);
            return;
        }

        room.gridPos = pos;
        room.isStartRoom = start;
        room.isBossRoom = boss;
        rooms[pos] = room;
    }

    void ConnectRooms(Vector2Int a, Vector2Int b)
    {
        DungeonRoom A = rooms[a];
        DungeonRoom B = rooms[b];

        Vector2Int dir = b - a;

        if (dir == Vector2Int.up)
        {
            ActivateDoorPair(
                A.doorUp, Door.DoorSide.Up, B,
                B.doorDown, Door.DoorSide.Down, A
            );
        }
        else if (dir == Vector2Int.down)
        {
            ActivateDoorPair(
                A.doorDown, Door.DoorSide.Down, B,
                B.doorUp, Door.DoorSide.Up, A
            );
        }
        else if (dir == Vector2Int.left)
        {
            ActivateDoorPair(
                A.doorLeft, Door.DoorSide.Left, B,
                B.doorRight, Door.DoorSide.Right, A
            );
        }
        else if (dir == Vector2Int.right)
        {
            ActivateDoorPair(
                A.doorRight, Door.DoorSide.Right, B,
                B.doorLeft, Door.DoorSide.Left, A
            );
        }
    }

    void ActivateDoorPair(
        GameObject doorObjA, Door.DoorSide sideA, DungeonRoom targetB,
        GameObject doorObjB, Door.DoorSide sideB, DungeonRoom targetA)
    {
        if (doorObjA == null || doorObjB == null)
        {
            Debug.LogError("DungeonGenerator: one of door GameObjects is null. Check DungeonRoom door refs.");
            return;
        }

        doorObjA.SetActive(true);
        doorObjB.SetActive(true);

        Door dA = doorObjA.GetComponent<Door>();
        Door dB = doorObjB.GetComponent<Door>();

        if (dA == null || dB == null)
        {
            Debug.LogError("DungeonGenerator: one of doors has no Door component.");
            return;
        }

        dA.side = sideA;
        dA.leadsTo = targetB;

        dB.side = sideB;
        dB.leadsTo = targetA;
    }
}
