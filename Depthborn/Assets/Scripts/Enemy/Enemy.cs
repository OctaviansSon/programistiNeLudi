using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3;
    public float speed = 2f;
    public int contactDamage = 1;

    Transform player;
    PlayerHealth health;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        health = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (!player) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            health.TakeDamage(contactDamage);
        }
    }

    public void Hit(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
            Destroy(gameObject);
    }
}
