using UnityEngine;

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
    public bool visited;
    public bool cleared;

    public void SpawnEnemies()
    {
        if (visited) return;
        if (spawnPoints == null || spawnPoints.Length == 0) {
            Debug.LogWarning($"Room {name} has no spawnPoints assigned!");
            visited = true;
            return;
        }
        if (enemies == null || enemies.Length == 0) {
            Debug.LogWarning($"Room {name} has no enemy prefabs!");
            visited = true;
            return;
        }

        visited = true;
        int count = Random.Range(2, 6);
        for (int i = 0; i < count; i++)
        {
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemies[Random.Range(0, enemies.Length)], sp.position, Quaternion.identity);
        }
    }
}
