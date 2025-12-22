using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Boss Settings")]
    public int baseBossHP = 20;
    public GameObject minionPrefab;
    public int minionsPerPhase = 3;
    public float spawnRadius = 1.5f;

    [Header("Boss Rewards")]
    public GameObject exitPortalPrefab;
    public ItemDropTable bossDropTable;

    int lastPhase = 4;
    SpriteRenderer sr;

    protected override void Start()
    {
        enemyAudio = GetComponent<EnemyAudio>();

        int floorBonus = RunManager.Instance != null
            ? RunManager.Instance.floor * 5
            : 0;

        hp = baseBossHP + floorBonus;

        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.green;

        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
    }

    public override void Hit(int dmg)
    {
        if (isDead) return;

        hp -= dmg;
        enemyAudio?.PlayHit();

        int currentPhase = Mathf.CeilToInt((float)hp / (baseBossHP) * 4f);

        if (currentPhase < lastPhase)
        {
            SpawnMinions();
            lastPhase = currentPhase;
        }

        if (hp <= 0)
            Die();
    }

    protected override void Die()
    {
        isDead = true;
        enemyAudio?.PlayDeath();

        SpawnBossReward();
        SpawnExitPortal();

        Debug.Log("🔥 BOSS DEFEATED 🔥");

        Destroy(gameObject, 0.2f);
    }

    void SpawnMinions()
    {
        if (minionPrefab == null) return;

        for (int i = 0; i < minionsPerPhase; i++)
        {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            Vector3 pos = transform.position + (Vector3)offset;

            Instantiate(minionPrefab, pos, Quaternion.identity, transform.parent);
        }
    }

    void SpawnBossReward()
    {
        if (bossDropTable == null || itemPickupPrefab == null) return;

        ItemData item = bossDropTable.GetRandomItem();
        if (item == null) return;

        GameObject p = Instantiate(itemPickupPrefab, transform.position, Quaternion.identity);
        p.GetComponent<ItemPickup>().item = item;
    }

    void SpawnExitPortal()
    {
        if (exitPortalPrefab == null) return;

        Instantiate(exitPortalPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
    }
}
