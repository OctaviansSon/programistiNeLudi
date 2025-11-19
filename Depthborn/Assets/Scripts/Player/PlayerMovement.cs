using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    Rigidbody2D rb;
    Vector2 input;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
    }
}
