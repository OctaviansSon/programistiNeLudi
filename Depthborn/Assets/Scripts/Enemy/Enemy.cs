using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int baseHP = 3;
    public float speed = 2f;
    public int contactDamage = 1;
    public float attackCooldown = 0.6f;

    [Header("Drop")]
    public ItemDropTable dropTable;
    public GameObject itemPickupPrefab;
    [Range(0f, 1f)] public float dropChance = 0.15f;

    protected EnemyAudio enemyAudio;
    protected Transform player;
    protected float lastAttackTime = -10f;

    protected int hp;
    public bool isDead = false;

    protected virtual void Start()
    {
        enemyAudio = GetComponent<EnemyAudio>();

        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;

        int hpBonus = RunManager.Instance != null
            ? RunManager.Instance.EnemyHPBonus()
            : 0;

        hp = baseHP + hpBonus;

        if (RunManager.Instance != null)
        {
            contactDamage = Mathf.RoundToInt(
                contactDamage * RunManager.Instance.enemyDamageMultiplier
            );
        }

    }

    protected virtual void Update()
    {
        if (isDead) return;

        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
            return;
        }

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

        TryDropItem();

        Destroy(gameObject, 0.1f);
    }

    void TryDropItem()
    {
        if (dropTable == null || itemPickupPrefab == null) return;
        if (Random.value > dropChance) return;

        ItemData item = dropTable.GetRandomItem();
        if (item == null) return;

        GameObject p = Instantiate(itemPickupPrefab, transform.position, Quaternion.identity);
        p.GetComponent<ItemPickup>().item = item;
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
