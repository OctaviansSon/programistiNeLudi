using UnityEngine;
using System.Collections;

public class RoomTransition : MonoBehaviour
{
    public DungeonGenerator gen;
    public DungeonRoom currentRoom;
    public SpriteRenderer darkMask;
    public float fadeDuration = 0.3f;

    void Start()
    {
        if (gen == null) gen = FindObjectOfType<DungeonGenerator>();
        currentRoom = gen.rooms.ContainsKey(Vector2Int.zero) ? gen.rooms[Vector2Int.zero] : null;
        if (currentRoom != null) EnterRoom(currentRoom);
    }

    public void EnterRoom(DungeonRoom room)
    {
        if (room == null) return;
        currentRoom = room;
        // place darkMask above room center
        if (darkMask != null)
            darkMask.transform.position = new Vector3(room.transform.position.x, room.transform.position.y, darkMask.transform.position.z);

        StartCoroutine(FadeIn());
        room.SpawnEnemies();
        CloseDoors(room);
    }

    void CloseDoors(DungeonRoom room)
    {
        Door[] doors = room.GetComponentsInChildren<Door>(true);
        foreach (Door d in doors)
            d.Close();
    }

    private void Update()
    {
        if (currentRoom == null) return;
        if (currentRoom.visited && !currentRoom.cleared)
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            // filter enemies inside current room bounds? for now assume global; fine for tests
            if (enemies.Length == 0)
            {
                currentRoom.cleared = true;
                foreach (Door d in currentRoom.GetComponentsInChildren<Door>(true))
                    d.Open();
            }
        }
    }

    IEnumerator FadeIn()
    {
        if (darkMask == null) yield break;
        float t = 0;
        Color c = darkMask.color;
        c.a = 1;
        darkMask.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, t / fadeDuration);
            darkMask.color = c;
            yield return null;
        }
        c.a = 0;
        darkMask.color = c;
    }
}
