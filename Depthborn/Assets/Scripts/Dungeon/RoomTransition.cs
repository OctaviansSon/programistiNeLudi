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

    // защита от параллельных переходов
    bool isTransitioning = false;

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

        if (!gen.rooms.TryGetValue(Vector2Int.zero, out currentRoom))
        {
            Debug.LogError("RoomTransition: no start room at (0,0)");
            return;
        }

        // заходим в стартовую комнату
        StartCoroutine(EnterRoomCoroutine(currentRoom, null));
    }

    // публичный вход — теперь корутина, чтобы ставить флаг перехода
    public void EnterRoom(DungeonRoom room, Door.DoorSide? fromSide)
    {
        if (isTransitioning) return;
        StartCoroutine(EnterRoomCoroutine(room, fromSide));
    }

    IEnumerator EnterRoomCoroutine(DungeonRoom room, Door.DoorSide? fromSide)
    {
        isTransitioning = true;

        DeactivateAllRooms();
        room.gameObject.SetActive(true);

        currentRoom = room;

        MoveCamera(room);

        Vector3 spawnPos = CalculatePlayerPosition(room, fromSide);
        if (player != null)
            player.position = spawnPos;

        // небольшая защита: если игрок пересекает зону двери — подталкиваем внутрь
        FixPlayerIfInsideDoor(room);

        // тёмная маска
        PlayFade(room);

        // спавн мобов (сам метод внутри комнаты защитит от повторного спавна)
        room.SpawnEnemies();

        // помечаем, что заходили
        room.visited = true;

        // закрываем/открываем двери
        if (!room.cleared)
            CloseDoors(room);
        else
            OpenDoors(room);

        // ждем конца кадра чтобы избежать мгновенных повторных срабатываний (безопасность)
        yield return null;
        isTransitioning = false;
    }

    // ============================================================
    //                   ПОЗИЦИЯ ИГРОКА
    // ============================================================
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
        {
            Debug.LogWarning($"RoomTransition: room {room.name} has no door for side {enterSide}");
            return room.transform.position;
        }

        Vector3 pos = door.transform.position;

        switch (enterSide)
        {
            case Door.DoorSide.Up:    pos += Vector3.down * 1.2f;  break;
            case Door.DoorSide.Down:  pos += Vector3.up * 1.2f;    break;
            case Door.DoorSide.Left:  pos += Vector3.right * 1.2f; break;
            case Door.DoorSide.Right: pos += Vector3.left * 1.2f;  break;
        }

        return pos;
    }

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

    Door FindDoor(DungeonRoom room, Door.DoorSide side)
    {
        Door[] doors = room.GetComponentsInChildren<Door>(true);
        foreach (var d in doors)
            if (d != null && d.side == side)
                return d;
        return null;
    }

    void MoveCamera(DungeonRoom room)
    {
        if (cam == null) return;
        Vector3 camPos = room.transform.position;
        camPos.z = camZ;
        cam.transform.position = camPos;
    }

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
    //                   ДВЕРИ (безопасно)
    // ============================================================
    void CloseDoors(DungeonRoom room)
    {
        foreach (Door d in room.GetComponentsInChildren<Door>(true))
        {
            if (d == null) continue;
            // перед закрытием убедимся, что игрок не окажется внутри блокирующего коллайдера
            d.Close();
        }
    }

    void OpenDoors(DungeonRoom room)
    {
        foreach (Door d in room.GetComponentsInChildren<Door>(true))
            if (d != null)
                d.Open();
    }

    // если игрок в момент закрытия окажется внутри блокирующего коллайдера - подтолкнём внутрь
    void FixPlayerIfInsideDoor(DungeonRoom room)
    {
        if (player == null) return;

        Collider2D playerCol = player.GetComponent<Collider2D>();
        foreach (Door d in room.GetComponentsInChildren<Door>(true))
        {
            if (d == null) continue;
            Collider2D block = d.blockCol;
            if (block == null) continue;

            // если игрок коллайдер пересекается с блоком (или точка внутри), отодвинем
            if (playerCol != null)
            {
                if (playerCol.IsTouching(block) || block.bounds.Contains(player.position))
                {
                    Vector3 dir = (room.transform.position - d.transform.position).normalized;
                    player.position += dir * 0.6f; // мягкий сдвиг внутрь
                }
            }
            else
            {
                if (block.bounds.Contains(player.position))
                {
                    Vector3 dir = (room.transform.position - d.transform.position).normalized;
                    player.position += dir * 0.6f;
                }
            }
        }
    }

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

        // если не заходили или уже очищена — выходим
        if (!currentRoom.visited || currentRoom.cleared) return;

        int alive = currentRoom.CountAliveEnemies();
        if (alive <= 0)
        {
            currentRoom.cleared = true;
            OpenDoors(currentRoom);
        }
    }
}
