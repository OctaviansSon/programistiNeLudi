using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    public int maxHP = 6;      // 6 = 3 сердца
    public int hp = 6;
    public int maxShield = 4;  // 4 = 2 щита
    public int shield = 0;

    [Header("Damage")]
    public float damageCooldown = 0.35f;
    bool canTakeDamage = true;

    public delegate void OnHealthChanged();
    public event OnHealthChanged HealthChanged;

    public void TakeDamage(int dmg)
    {
        if (!canTakeDamage) return;
        StartCoroutine(DamageCD());

        // сначала урон уходит в щиты
        if (shield > 0)
        {
            shield -= dmg;
            if (shield < 0)
                hp += shield; // shield был отрицательный → уходит в HP
        }
        else
        {
            hp -= dmg;
        }

        hp = Mathf.Clamp(hp, 0, maxHP);
        shield = Mathf.Clamp(shield, 0, maxShield);

        HealthChanged?.Invoke();

        if (hp <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        hp += amount;
        hp = Mathf.Clamp(hp, 0, maxHP);
        HealthChanged?.Invoke();
    }

    public void AddShield(int amount)
    {
        shield += amount;
        shield = Mathf.Clamp(shield, 0, maxShield);
        HealthChanged?.Invoke();
    }

    System.Collections.IEnumerator DamageCD()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    void Die()
    {
        Debug.Log("💀 Player died!");
        FindFirstObjectByType<DeathMenu>().Show();
        gameObject.SetActive(false);
    }
}
