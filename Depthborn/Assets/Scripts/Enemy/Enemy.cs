using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int maxHP = 3;

    Transform player;
    int currentHP;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHP = maxHP;
    }

    void Update()
    {
        if (!player) return;
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );
    }

    public void Hit(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0) Destroy(gameObject);
    }
}
