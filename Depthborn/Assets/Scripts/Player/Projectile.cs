using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 1;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 dir)
    {
        rb.linearVelocity = dir * speed;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Enemy e))
        {
            e.Hit(damage);
            Destroy(gameObject);
            return;
        }

        // чтобы не убивать пули о стены — проверяем тег
        if (col.CompareTag("Wall"))
            Destroy(gameObject);
    }
}
