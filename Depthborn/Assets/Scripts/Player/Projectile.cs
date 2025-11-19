using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 dir)
    {
        rb.linearVelocity = dir * speed;
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Enemy e))
            e.Hit(damage);

        Destroy(gameObject);
    }
}
