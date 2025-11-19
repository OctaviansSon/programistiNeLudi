using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3;
    public float speed = 2f;

    Transform player;

    void Start() =>
        player = GameObject.FindWithTag("Player").transform;

    void Update()
    {
        if (!player) return;
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }

    public void Hit(int dmg)
    {
        hp -= dmg;
        Debug.Log($"☠ Enemy HP = {hp}");

        if (hp <= 0) Destroy(gameObject);
    }
}
