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

    // спавним врагов только раз (контролируем флаг enemiesSpawned отдельно от visited)
    public void SpawnEnemies()
    {
        if (enemiesSpawned) return;

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning($"Room {name} has no spawnPoints assigned!");
            enemiesSpawned = true; // чтобы не пытаться снова
            return;
        }
        if (enemies == null || enemies.Length == 0)
        {
            Debug.LogWarning($"Room {name} has no enemy prefabs!");
            enemiesSpawned = true;
            return;
        }

        enemiesSpawned = true;

        int count = Random.Range(2, 6); // можешь менять
        for (int i = 0; i < count; i++)
        {
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            if (sp == null) continue;
            GameObject e = Instantiate(enemies[Random.Range(0, enemies.Length)], sp.position, Quaternion.identity, transform);
            spawnedEnemies.Add(e);
        }
    }

    // корректный подсчёт "живых" врагов в комнате
    public int CountAliveEnemies()
    {
        // обновим список созданных
        spawnedEnemies.RemoveAll(x => x == null);

        int count = spawnedEnemies.Count;

        // плюс любые сторонние враги, которые могут быть добавлены вручную (сцена)
        Enemy[] other = GetComponentsInChildren<Enemy>(true);
        foreach (var en in other)
        {
            if (en == null) continue;
            // если этот объект не в нашем spawnedEnemies - посчитаем его
            if (!spawnedEnemies.Contains(en.gameObject))
            {
                if (en.gameObject.activeInHierarchy)
                    count++;
            }
        }

        return count;
    }
}
