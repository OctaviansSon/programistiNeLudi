using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartUIController : MonoBehaviour
{
    public PlayerHealth player;

    [Header("Sprites")]
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    [Header("UI")]
    public GameObject heartPrefab;
    public Transform heartsParent;

    List<Image> hearts = new();

    void Start()
    {
        GenerateHearts();
        player.HealthChanged += UpdateHearts;
    }

    void GenerateHearts()
    {
        foreach (Transform t in heartsParent)
            Destroy(t.gameObject);

        hearts.Clear();

        int heartCount = player.maxHP / 2;

        for (int i = 0; i < heartCount; i++)
        {
            GameObject h = Instantiate(heartPrefab, heartsParent);
            Image img = h.GetComponent<Image>();
            hearts.Add(img);
        }
    }
    public void Rebuild()
    {
        GenerateHearts();
        UpdateHearts();
    }

    void UpdateHearts()
    {
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
                hp = 0;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
