using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartUIController : MonoBehaviour
{
    PlayerHealth player;

    [Header("Sprites")]
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    [Header("UI")]
    public GameObject heartPrefab;
    public Transform heartsParent;

    List<Image> hearts = new();

    void OnEnable()
    {
        PlayerSpawner.OnPlayerSpawned += Init;
    }

    void OnDisable()
    {
        PlayerSpawner.OnPlayerSpawned -= Init;

        if (player != null)
            player.HealthChanged -= Rebuild;
    }

    void Init(PlayerHealth ph)
    {
        player = ph;
        player.HealthChanged += Rebuild;

        Rebuild();
    }

    void Rebuild()
    {
        if (player == null) return;

        GenerateHearts();
        UpdateHearts();
    }

    void GenerateHearts()
    {
        foreach (Transform t in heartsParent)
            Destroy(t.gameObject);

        hearts.Clear();

        int count = player.maxHP / 2;

        for (int i = 0; i < count; i++)
        {
            Image img = Instantiate(heartPrefab, heartsParent).GetComponent<Image>();
            hearts.Add(img);
        }
    }

    void UpdateHearts()
    {
        int hp = player.hp;

        foreach (var h in hearts)
        {
            if (hp >= 2) { h.sprite = fullHeart; hp -= 2; }
            else if (hp == 1) { h.sprite = halfHeart; hp = 0; }
            else h.sprite = emptyHeart;
        }
    }
}
