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

    [Header("Shield Sprites")]
    public Sprite fullShield;
    public Sprite halfShield;
    public Sprite emptyShield;

    public GameObject heartPrefab;
    public Transform heartsParent;

    List<Image> hearts = new List<Image>();

    void Start()
    {
        player = FindFirstObjectByType<PlayerHealth>();
        player.HealthChanged += UpdateHearts;

        GenerateHearts();
        UpdateHearts();
    }

    void GenerateHearts()
    {
        hearts.Clear();

        foreach (Transform child in heartsParent)
            Destroy(child.gameObject);

        int totalHearts = player.maxHP / 2;

        for (int i = 0; i < totalHearts; i++)
        {
            GameObject h = Instantiate(heartPrefab, heartsParent);
            hearts.Add(h.GetComponent<Image>());
        }
    }

    public void UpdateHearts()
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
                hp -= 1;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
