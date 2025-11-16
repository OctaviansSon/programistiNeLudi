using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.15f;

    [Header("Combat")]
    public int damage = 1;
    public float attackCooldown = 0.3f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDashing = false;
    private float lastAttackTime = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
            StartCoroutine(Dash());

        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime > attackCooldown)
            Attack();
    }

    void FixedUpdate()
    {
        if (!isDashing)
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        // позже здесь будет анимация оружия или удара
        Debug.Log("⚔ Player swing!");
    }

    System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        float t = 0;

        while (t < dashDuration)
        {
            rb.linearVelocity = moveInput * dashSpeed;
            t += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        isDashing = false;
    }
}
