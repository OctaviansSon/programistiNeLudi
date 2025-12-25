using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShieldUIController : MonoBehaviour
{
    PlayerHealth player;

    public Sprite fullShield;
    public Sprite halfShield;
    public Sprite emptyShield;

    public GameObject shieldPrefab;
    public Transform shieldsParent;

    List<Image> shields = new();

    void OnEnable()
    {
        PlayerSpawner.OnPlayerSpawned += Init;
    }

    void OnDisable()
    {
        PlayerSpawner.OnPlayerSpawned -= Init;

        if (player != null)
            player.HealthChanged -= UpdateShields;
    }

    void Init(PlayerHealth ph)
    {
        player = ph;
        player.HealthChanged += UpdateShields;

        GenerateShields();
        UpdateShields();
    }

    void GenerateShields()
    {
        foreach (Transform t in shieldsParent)
            Destroy(t.gameObject);

        shields.Clear();

        int count = player.maxShield / 2;

        for (int i = 0; i < count; i++)
        {
            Image img = Instantiate(shieldPrefab, shieldsParent).GetComponent<Image>();
            shields.Add(img);
        }
    }

    void UpdateShields()
    {
        int shield = player.shield;

        foreach (var s in shields)
        {
            if (shield >= 2) { s.sprite = fullShield; shield -= 2; }
            else if (shield == 1) { s.sprite = halfShield; shield = 0; }
            else s.sprite = emptyShield;
        }
    }
}
