using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public int roomCount = 8;
    public float spacing = 14f;

    public Dictionary<Vector2Int, DungeonRoom> rooms = new();

    Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Vector2Int current = Vector2Int.zero;
        CreateRoom(current, true, false);

        for (int i = 1; i < roomCount; i++)
        {
            Vector2Int next;
            int tries = 0;

            do
            {
                tries++;
                next = current + directions[Random.Range(0, 4)];
            }
            while (rooms.ContainsKey(next) && tries < 50);

            if (rooms.ContainsKey(next))
            {
                // слишком долго — ищем свободную позицию рудиментарно
                bool found = false;
                for (int x=-roomCount; x<=roomCount && !found; x++)
                    for (int y=-roomCount; y<=roomCount && !found; y++)
                        if (!rooms.ContainsKey(new Vector2Int(x,y))) { next = new Vector2Int(x,y); found = true; }
            }

            CreateRoom(next, false, i == roomCount - 1);
            ConnectRooms(current, next);
            current = next;
        }
    }

    void CreateRoom(Vector2Int pos, bool start, bool boss)
    {
        Vector3 worldPos = new Vector3(pos.x * spacing, pos.y * spacing, 0);
        GameObject r = Instantiate(roomPrefab, worldPos, Quaternion.identity);
        DungeonRoom room = r.GetComponent<DungeonRoom>();
        room.gridPos = pos;
        room.isStartRoom = start;
        room.isBossRoom = boss;
        rooms[pos] = room;
    }

    void ConnectRooms(Vector2Int a, Vector2Int b)
    {
        Vector2Int dir = b - a;

        if (!rooms.ContainsKey(a) || !rooms.ContainsKey(b)) return;

        DungeonRoom R1 = rooms[a];
        DungeonRoom R2 = rooms[b];

        if (dir == Vector2Int.up)
        {
            if (R1.doorUp) { R1.doorUp.SetActive(true); R1.doorUp.GetComponent<Door>().leadsTo = R2; }
            if (R2.doorDown) { R2.doorDown.SetActive(true); R2.doorDown.GetComponent<Door>().leadsTo = R1; }
        }
        else if (dir == Vector2Int.down)
        {
            if (R1.doorDown) { R1.doorDown.SetActive(true); R1.doorDown.GetComponent<Door>().leadsTo = R2; }
            if (R2.doorUp) { R2.doorUp.SetActive(true); R2.doorUp.GetComponent<Door>().leadsTo = R1; }
        }
        else if (dir == Vector2Int.left)
        {
            if (R1.doorLeft) { R1.doorLeft.SetActive(true); R1.doorLeft.GetComponent<Door>().leadsTo = R2; }
            if (R2.doorRight) { R2.doorRight.SetActive(true); R2.doorRight.GetComponent<Door>().leadsTo = R1; }
        }
        else if (dir == Vector2Int.right)
        {
            if (R1.doorRight) { R1.doorRight.SetActive(true); R1.doorRight.GetComponent<Door>().leadsTo = R2; }
            if (R2.doorLeft) { R2.doorLeft.SetActive(true); R2.doorLeft.GetComponent<Door>().leadsTo = R1; }
        }
    }
}
