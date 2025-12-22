using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    public int hp = 3;
    public float speed = 2f;
    public int contactDamage = 1;
    public float attackCooldown = 0.6f;

    protected EnemyAudio enemyAudio; // ← ПЕРЕИМЕНОВАЛИ
    protected Transform player;
    protected float lastAttackTime = -10f;

    public bool isDead = false;

    protected virtual void Start()
    {
        enemyAudio = GetComponent<EnemyAudio>();

        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
    }

    protected virtual void Update()
    {
        if (isDead || !player) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }

    public virtual void Hit(int dmg)
    {
        if (isDead) return;

        hp -= dmg;
        enemyAudio?.PlayHit();

        if (hp <= 0)
            Die();
    }

    protected virtual void Die()
    {
        isDead = true;
        enemyAudio?.PlayDeath();

        Destroy(gameObject, 0.1f);
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
