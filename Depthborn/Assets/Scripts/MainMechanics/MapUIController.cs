using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapUIController : MonoBehaviour
{
    public DungeonGenerator generator;
    public RoomTransition roomTransition;

    public GameObject roomIconPrefab;
    public RectTransform gridParent;
    public float cellSize = 24f;

    Dictionary<Vector2Int, Image> icons = new();

    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        UpdateMap();
    }

    void GenerateMap()
    {
        foreach (Transform t in gridParent)
            Destroy(t.gameObject);

        icons.Clear();

        foreach (var kv in generator.rooms)
        {
            Vector2Int gridPos = kv.Key;

            GameObject iconObj = Instantiate(roomIconPrefab, gridParent);
            RectTransform rt = iconObj.GetComponent<RectTransform>();
            Image img = iconObj.GetComponent<Image>();

            // ❗ ВАЖНО: позиция по gridPos
            rt.anchoredPosition = new Vector2(
                gridPos.x * cellSize,
                gridPos.y * cellSize
            );

            img.color = Color.gray;
            icons.Add(gridPos, img);
        }
    }

    void UpdateMap()
    {
        foreach (var kv in generator.rooms)
        {
            DungeonRoom room = kv.Value;
            Image img = icons[kv.Key];

            if (room == roomTransition.currentRoom)
                img.color = Color.yellow;
            else if (room.visited)
                img.color = room.isBossRoom ? Color.red : Color.white;
            else
                img.color = Color.gray;
        }
    }
}
