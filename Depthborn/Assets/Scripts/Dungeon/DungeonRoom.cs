using UnityEngine;
using System.Collections.Generic;

public class DungeonRoom : MonoBehaviour
{
    public Vector2Int gridPos;
    public bool isStartRoom;
    public bool isBossRoom;

    [Header("Doors")]
    public GameObject doorUp;
    public GameObject doorDown;
    public GameObject doorLeft;
    public GameObject doorRight;

    [Header("Enemy Spawn")]
    public Transform[] spawnPoints;
    public GameObject[] enemies;
    [HideInInspector] public bool visited;
    [HideInInspector] public bool cleared;
    public Transform playerSpawn;

    [Header("Visual")]
    public SpriteRenderer darkMask; // назначай в инспекторе (child DarkMask)

    void Awake()
    {
        // fallback для spawnPoints — ищем по тегу или по имени
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            List<Transform> found = new List<Transform>();
            foreach (Transform t in GetComponentsInChildren<Transform>(true))
            {
                if (t == transform) continue;
                if (t.CompareTag("SpawnPoint") || t.name.ToLower().Contains("spawn"))
                    found.Add(t);
            }

            if (found.Count > 0)
                spawnPoints = found.ToArray();
        }

        if (darkMask == null)
            darkMask = transform.Find("DarkMask")?.GetComponent<SpriteRenderer>();
    }

    public void SpawnEnemies()
    {
        if (visited) return;

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning($"Room {name} has no spawnPoints assigned!");
            visited = true;
            return;
        }
        if (enemies == null || enemies.Length == 0)
        {
            Debug.LogWarning($"Room {name} has no enemy prefabs!");
            visited = true;
            return;
        }

        visited = true;
        int count = Random.Range(2, 6);
        for (int i = 0; i < count; i++)
        {
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            if (sp == null) continue;
            GameObject e = Instantiate(enemies[Random.Range(0, enemies.Length)], sp.position, Quaternion.identity);
            e.transform.SetParent(transform); // держим врагов внутри комнаты в иерархии
        }
    }
}
