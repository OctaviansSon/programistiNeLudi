using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomTransition : MonoBehaviour
{
    public DungeonGenerator gen;
    public DungeonRoom currentRoom;
    public Camera cam;
    public float camZ = -10f;
    public float fadeDuration = 0.3f;
    public Transform player;

    void Start()
    {
        if (gen == null) gen = FindFirstObjectByType<DungeonGenerator>();
        if (cam == null) cam = Camera.main;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (gen == null)
        {
            Debug.LogError("RoomTransition: DungeonGenerator not found!");
            enabled = false;
            return;
        }

        DeactivateAllRooms();

        // стартовая точка — (0,0)
        if (!gen.rooms.TryGetValue(Vector2Int.zero, out currentRoom))
        {
            Debug.LogError("RoomTransition: no start room at (0,0)");
            return;
        }

        EnterRoom(currentRoom, null);
    }

    // ============================================================
    //                   ВХОД В КОМНАТУ
    // ============================================================
    public void EnterRoom(DungeonRoom room, Door.DoorSide? fromSide)
    {
        DeactivateAllRooms();
        room.gameObject.SetActive(true);

        currentRoom = room;
        MoveCamera(room);

        Vector3 spawnPos = CalculatePlayerPosition(room, fromSide);
        if (player != null)
            player.position = spawnPos;

        PlayFade(room);

        // ВАЖНО!!! СНАЧАЛА СПАВН, ПОТОМ visited
        room.SpawnEnemies();
        room.visited = true;

        if (!room.cleared)
            CloseDoors(room);
        else
            OpenDoors(room);
    }


    // ============================================================
    //                   ПОЗИЦИЯ ИГРОКА
    // ============================================================
    Vector3 CalculatePlayerPosition(DungeonRoom room, Door.DoorSide? fromSide)
    {
        // Первое появление — спавн по playerSpawn
        if (!fromSide.HasValue)
        {
            if (room.playerSpawn != null)
                return room.playerSpawn.position;

            return room.transform.position; // fallback
        }

        // Зашли с правой — появляемся у левой двери комнаты
        // Зашли с левой — у правой
        // Зашли сверху — у нижней
        // Зашли снизу — у верхней
        Door.DoorSide enterSide = Opposite(fromSide.Value);

        // Ищем нужную дверь внутри новой комнаты
        Door door = FindDoor(room, enterSide);

        if (door == null)
        {
            Debug.LogWarning($"RoomTransition: room {room.name} has no door for side {enterSide}");
            return room.transform.position;
        }

        Vector3 pos = door.transform.position;

        // Сдвиг внутрь комнаты от двери
        switch (enterSide)
        {
            case Door.DoorSide.Up:    pos += Vector3.down * 1.2f;  break;
            case Door.DoorSide.Down:  pos += Vector3.up * 1.2f;    break;
            case Door.DoorSide.Left:  pos += Vector3.right * 1.2f; break;
            case Door.DoorSide.Right: pos += Vector3.left * 1.2f;  break;
        }

        return pos;
    }

    // ============================================================
    //                   ОТРАЖЕНИЕ СТОРОНЫ
    // ============================================================
    Door.DoorSide Opposite(Door.DoorSide s)
    {
        switch (s)
        {
            case Door.DoorSide.Up:    return Door.DoorSide.Down;
            case Door.DoorSide.Down:  return Door.DoorSide.Up;
            case Door.DoorSide.Left:  return Door.DoorSide.Right;
            case Door.DoorSide.Right: return Door.DoorSide.Left;
        }
        return s;
    }

    // ============================================================
    //                   ПОИСК ДВЕРИ
    // ============================================================
    Door FindDoor(DungeonRoom room, Door.DoorSide side)
    {
        Door[] doors = room.GetComponentsInChildren<Door>(true);

        foreach (var d in doors)
            if (d.side == side)
                return d;

        return null;
    }

    // ============================================================
    //                   КАМЕРА
    // ============================================================
    void MoveCamera(DungeonRoom room)
    {
        if (cam == null) return;

        Vector3 camPos = room.transform.position;
        camPos.z = camZ;
        cam.transform.position = camPos;
    }

    // ============================================================
    //                   ТЁМНАЯ МАСКА
    // ============================================================
    void PlayFade(DungeonRoom room)
    {
        if (room.darkMask != null)
        {
            room.darkMask.color = new Color(0, 0, 0, 1f);
            StartCoroutine(FadeMask(room.darkMask));
        }
    }

    IEnumerator FadeMask(SpriteRenderer mask)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            mask.color = new Color(0, 0, 0, a);
            yield return null;
        }
        mask.color = new Color(0, 0, 0, 0f);
    }

    // ============================================================
    //                   ДВЕРИ
    // ============================================================
    void CloseDoors(DungeonRoom room)
    {
        foreach (Door d in room.GetComponentsInChildren<Door>(true))
            d.Close();
    }

    void OpenDoors(DungeonRoom room)
    {
        foreach (Door d in room.GetComponentsInChildren<Door>(true))
            d.Open();
    }

    // ============================================================
    //                   ДЕАКТИВАЦИЯ ВСЕХ КОМНАТ
    // ============================================================
    void DeactivateAllRooms()
    {
        foreach (var kv in gen.rooms)
            if (kv.Value != null)
                kv.Value.gameObject.SetActive(false);
    }

    // ============================================================
    //                   ОБНОВЛЕНИЕ / ОЧИСТКА КОМНАТЫ
    // ============================================================
    void Update()
    {
        if (currentRoom == null) return;
        if (!currentRoom.visited || currentRoom.cleared) return;

        Enemy[] enemies = currentRoom.GetComponentsInChildren<Enemy>(true);
        bool anyAlive = false;

        foreach (var e in enemies)
        {
            if (e != null)
            {
                anyAlive = true;
                break;
            }
        }

        if (!anyAlive)
        {
            currentRoom.cleared = true;
            OpenDoors(currentRoom);
        }
    }
}
