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

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
    }

    void Update()
    {
        if (!player) return;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    public void Hit(int dmg)
    {
        hp -= dmg;
        Debug.Log($"☠ Enemy HP = {hp}");

        if (hp <= 0) Destroy(gameObject);
    }

    // Если хочешь наносить урон при пересечении коллайдеров — используем OnCollisionEnter2D или OnTriggerStay2D с таймером
    void OnCollisionStay2D(Collision2D col)
    {
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
