using UnityEngine;
using System.Collections;

public class RoomTransition : MonoBehaviour
{
    public DungeonGenerator gen;
    public DungeonRoom currentRoom;
    public Transform player;

    bool isTransitioning = false;

    void Start()
    {
        if (gen == null) gen = FindFirstObjectByType<DungeonGenerator>();

        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (gen == null)
        {
            Debug.LogError("RoomTransition: DungeonGenerator not found!");
            enabled = false;
            return;
        }

        DeactivateAllRooms();

        if (!gen.rooms.TryGetValue(Vector2Int.zero, out currentRoom))
        {
            Debug.LogError("RoomTransition: no start room at (0,0)");
            return;
        }

        StartCoroutine(EnterRoomCoroutine(currentRoom, null));
    }


    public void EnterRoom(DungeonRoom room, Door.DoorSide? fromSide)
    {
        if (isTransitioning) return;
        StartCoroutine(EnterRoomCoroutine(room, fromSide));
    }

    IEnumerator EnterRoomCoroutine(DungeonRoom room, Door.DoorSide? fromSide)
    {
        isTransitioning = true;

        // FADE OUT
        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.FadeOut();

        // FREEZE
        Time.timeScale = 0f;

        // SWITCH ROOM
        DeactivateAllRooms();
        room.gameObject.SetActive(true);
        currentRoom = room;

        Vector3 spawnPos = CalculatePlayerPosition(room, fromSide);
        if (player != null)
            player.position = spawnPos;

        room.SpawnEnemies();
        room.visited = true;

        if (!room.cleared)
            CloseDoors(room);
        else
            OpenDoors(room);

        // FADE IN
        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.FadeIn();

        // UNFREEZE
        Time.timeScale = 1f;

        isTransitioning = false;
    }


    // ================= POSITION =================

    Vector3 CalculatePlayerPosition(DungeonRoom room, Door.DoorSide? fromSide)
    {
        if (!fromSide.HasValue)
        {
            if (room.playerSpawn != null)
                return room.playerSpawn.position;
            return room.transform.position;
        }

        Door.DoorSide enterSide = Opposite(fromSide.Value);
        Door door = FindDoor(room, enterSide);

        if (door == null)
            return room.transform.position;

        Vector3 pos = door.transform.position;

        switch (enterSide)
        {
            case Door.DoorSide.Up: pos += Vector3.down * 1.2f; break;
            case Door.DoorSide.Down: pos += Vector3.up * 1.2f; break;
            case Door.DoorSide.Left: pos += Vector3.right * 1.2f; break;
            case Door.DoorSide.Right: pos += Vector3.left * 1.2f; break;
        }

        return pos;
    }

    Door.DoorSide Opposite(Door.DoorSide s)
    {
        return s switch
        {
            Door.DoorSide.Up => Door.DoorSide.Down,
            Door.DoorSide.Down => Door.DoorSide.Up,
            Door.DoorSide.Left => Door.DoorSide.Right,
            Door.DoorSide.Right => Door.DoorSide.Left,
            _ => s
        };
    }

    Door FindDoor(DungeonRoom room, Door.DoorSide side)
    {
        foreach (var d in room.GetComponentsInChildren<Door>(true))
            if (d.side == side) return d;
        return null;
    }


    void CloseDoors(DungeonRoom room)
    {
        foreach (var d in room.GetComponentsInChildren<Door>(true))
            d.Close();
    }

    void OpenDoors(DungeonRoom room)
    {
        foreach (var d in room.GetComponentsInChildren<Door>(true))
            d.Open();
    }

    void DeactivateAllRooms()
    {
        foreach (var r in gen.rooms.Values)
            r.gameObject.SetActive(false);
    }

    void Update()
    {
        if (currentRoom == null || !currentRoom.visited || currentRoom.cleared) return;

        if (currentRoom.CountAliveEnemies() <= 0)
        {
            currentRoom.cleared = true;
            OpenDoors(currentRoom);
        }
    }
}
