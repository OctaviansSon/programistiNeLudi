using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 6;
    public int hp;

    void Start() => hp = maxHP;

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log($"❤️ HP: {hp}");

        if (hp <= 0) Die();
    }

    void Die()
    {
        Debug.Log("💀 PLAYER DEAD!");
        Destroy(gameObject);
    }
}
