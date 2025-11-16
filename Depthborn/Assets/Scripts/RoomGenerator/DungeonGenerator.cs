using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public int gridSize = 3;
    public float roomSpacing = 12f;

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (Random.value < 0.45f) continue; // дырки в карте

                Vector3 pos = new Vector3(
                    x * roomSpacing,
                    y * roomSpacing,
                    0
                );

                Instantiate(roomPrefab, pos, Quaternion.identity);
            }
        }
    }
}
