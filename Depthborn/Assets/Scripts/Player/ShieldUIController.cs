using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShieldUIController : MonoBehaviour
{
    public PlayerHealth player;

    [Header("Sprites")]
    public Sprite fullShield;
    public Sprite halfShield;
    public Sprite emptyShield;

    [Header("UI")]
    public GameObject shieldPrefab;
    public Transform shieldsParent;

    List<Image> shields = new();

    void Start()
    {
        GenerateShields();
        player.HealthChanged += UpdateShields;
    }

    void GenerateShields()
    {
        foreach (Transform t in shieldsParent)
            Destroy(t.gameObject);

        shields.Clear();

        int shieldCount = player.maxShield / 2;

        for (int i = 0; i < shieldCount; i++)
        {
            GameObject s = Instantiate(shieldPrefab, shieldsParent);
            Image img = s.GetComponent<Image>();
            shields.Add(img);
        }
    }

    void UpdateShields()
    {
        int shield = player.shield;

        for (int i = 0; i < shields.Count; i++)
        {
            if (shield >= 2)
            {
                shields[i].sprite = fullShield;
                shield -= 2;
            }
            else if (shield == 1)
            {
                shields[i].sprite = halfShield;
                shield = 0;
            }
            else
            {
                shields[i].sprite = emptyShield;
            }
        }
    }
}
