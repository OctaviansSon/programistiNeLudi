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

    void Start()
    {
        hp = Mathf.Clamp(hp, 0, maxHP);
        shield = Mathf.Clamp(shield, 0, maxShield);
        HealthChanged?.Invoke();
    }

    public void TakeDamage(int dmg)
    {
        if (!canTakeDamage) return;
        StartCoroutine(DamageCD());

        if (shield > 0)
        {
            shield -= dmg;

            if (shield < 0)
                hp += shield; // shield отрицательный → остаток урона по HP
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
        hp = Mathf.Clamp(hp + amount, 0, maxHP);
        HealthChanged?.Invoke();
    }

    public void AddShield(int amount)
    {
        shield = Mathf.Clamp(shield + amount, 0, maxShield);
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
        var deathMenu = FindFirstObjectByType<DeathMenu>();
        if (deathMenu != null)
            deathMenu.Show();

        gameObject.SetActive(false);
    }
}
