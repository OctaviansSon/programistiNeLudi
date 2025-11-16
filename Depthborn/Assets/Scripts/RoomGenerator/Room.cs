using UnityEngine;

public class Room : MonoBehaviour
{
    public bool visited = false;
    public GameObject[] enemyPrefabs;
    public int enemyCountMin = 2;
    public int enemyCountMax = 5;

    public void SpawnEnemies()
    {
        if (visited) return;
        visited = true;

        int count = Random.Range(enemyCountMin, enemyCountMax + 1);
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle * 2f;
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], pos, Quaternion.identity);
        }
    }
}
