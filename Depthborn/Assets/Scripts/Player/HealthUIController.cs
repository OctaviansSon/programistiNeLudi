using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartUIController : MonoBehaviour
{
    public PlayerHealth player;

    [Header("Heart Sprites")]
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public GameObject heartPrefab; // должен содержать Image на корне
    public Transform heartsParent; // RectTransform в Canvas (не дочерний у DontDestroyOnLoad объектов)

    List<Image> hearts = new List<Image>();

    void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerHealth>();

        if (player == null)
        {
            Debug.LogError("HeartUIController: PlayerHealth not found in scene.");
            return;
        }

        player.HealthChanged += UpdateHearts;

        GenerateHearts();
        UpdateHearts();
    }

    void OnDestroy()
    {
        if (player != null)
            player.HealthChanged -= UpdateHearts;
    }

    void GenerateHearts()
    {
        hearts.Clear();

        // Очистим только дочерние элементы, если parent принадлежит активной сцене
        if (heartsParent == null)
        {
            Debug.LogError("HeartUIController: heartsParent not assigned!");
            return;
        }

        foreach (Transform child in heartsParent)
            Destroy(child.gameObject);

        int totalHearts = Mathf.CeilToInt(player.maxHP / 2.0f);

        for (int i = 0; i < totalHearts; i++)
        {
            GameObject h = Instantiate(heartPrefab, heartsParent);
            Image img = h.GetComponent<Image>();
            if (img == null)
                img = h.AddComponent<Image>();
            hearts.Add(img);
        }
    }

    public void UpdateHearts()
    {
        if (hearts == null || player == null) return;

        int hp = player.hp;
        for (int i = 0; i < hearts.Count; i++)
        {
            if (hp >= 2)
            {
                hearts[i].sprite = fullHeart;
                hp -= 2;
            }
            else if (hp == 1)
            {
                hearts[i].sprite = halfHeart;
                hp -= 1;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
