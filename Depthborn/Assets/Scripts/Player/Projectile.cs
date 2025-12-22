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
        Vector3 p = transform.position; p.z = 0; transform.position = p;
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

        if (col.CompareTag("Wall"))
            Destroy(gameObject);
    }
}
