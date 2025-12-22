using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerHealth health;
    public PlayerMovement movement;

    public float damageMultiplier = 1f;

    public void ApplyItem(ItemData item)
    {
        if (item.addHP > 0)
            health.maxHP += item.addHP;

        if (item.addShield > 0)
            health.AddShield(item.addShield);

        if (item.moveSpeedBonus != 0)
            movement.moveSpeed += item.moveSpeedBonus;

        damageMultiplier *= item.damageMultiplier;

        Debug.Log($"🟢 Picked item: {item.itemName}");
    }
}
