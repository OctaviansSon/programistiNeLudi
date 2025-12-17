using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    public int hp = 3;
    public float speed = 2f;
    public int contactDamage = 1;
    public float attackCooldown = 0.6f;

    Transform player;
    float lastAttackTime = -10f;

    // ключевой флаг для системы дверей
    public bool isDead = false;

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
    }

    void Update()
    {
        if (isDead) return; // мёртвый враг не двигается
        if (!player) return;

        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    public void Hit(int dmg)
    {
        if (isDead) return;

        hp -= dmg;

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // выключаем объект сразу чтобы CountAliveEnemies() работал моментально
        gameObject.SetActive(false);

        // уничтожаем после кадра
        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (isDead) return;

        if (col.collider.CompareTag("Player"))
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                var ph = col.collider.GetComponent<PlayerHealth>();
                if (ph != null) ph.TakeDamage(contactDamage);
                lastAttackTime = Time.time;
            }
        }
    }
}
