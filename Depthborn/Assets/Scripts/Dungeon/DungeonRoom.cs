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
    public GameObject[] enemies; // префабы
    [HideInInspector] public bool visited;
    [HideInInspector] public bool cleared;
    [HideInInspector] public bool enemiesSpawned;
    public Transform playerSpawn;

    [Header("Visual")]
    public SpriteRenderer darkMask;
    [Header("Boss")]
    public GameObject bossPrefab;

    // локальный список созданных врагов (для корректного подсчёта)
    List<GameObject> spawnedEnemies = new List<GameObject>();

    void Awake()
    {
        // fallback для spawnPoints
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
        if (enemiesSpawned) return;

        enemiesSpawned = true;

        if (isBossRoom)
        {
            SpawnBoss();
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0) return;
        if (enemies == null || enemies.Length == 0) return;

        int count = Random.Range(2, 6);
        for (int i = 0; i < count; i++)
        {
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject e = Instantiate(enemies[Random.Range(0, enemies.Length)], sp.position, Quaternion.identity, transform);
            spawnedEnemies.Add(e);
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab == null)
        {
            Debug.LogError($"BossRoom {name}: bossPrefab NOT assigned!");
            return;
        }

        Transform sp = (spawnPoints != null && spawnPoints.Length > 0)
            ? spawnPoints[Random.Range(0, spawnPoints.Length)]
            : transform;

        GameObject boss = Instantiate(bossPrefab, sp.position, Quaternion.identity, transform);
        spawnedEnemies.Add(boss);
    }



    public int CountAliveEnemies()
    {
        spawnedEnemies.RemoveAll(x => x == null);

        int count = spawnedEnemies.Count;

        Enemy[] other = GetComponentsInChildren<Enemy>(true);
        foreach (var en in other)
        {
            if (en == null) continue;
            if (!spawnedEnemies.Contains(en.gameObject))
            {
                if (en.gameObject.activeInHierarchy)
                    count++;
            }
        }

        return count;
    }
}
