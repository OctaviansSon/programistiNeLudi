using UnityEngine;

public enum ItemType
{
    Passive,
    Active
}

[CreateAssetMenu(menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType type;

    [Header("Bonuses")]
    public int addHP;
    public int addShield;
    public float moveSpeedBonus;
    public float damageMultiplier = 1f;

    [TextArea]
    public string description;
}
