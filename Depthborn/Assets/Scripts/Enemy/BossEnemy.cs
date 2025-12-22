using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Boss Settings")]
    public int maxHp = 20;
    public GameObject minionPrefab;
    public int minionsPerPhase = 3;
    public float spawnRadius = 1.5f;

    int lastPhase = 4;
    SpriteRenderer sr;

    protected override void Start()
    {
        base.Start();

        hp = maxHp;

        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.green;
    }

    public override void Hit(int dmg)
    {
        if (isDead) return;

        hp -= dmg;
        enemyAudio?.PlayHit();

        int currentPhase = Mathf.CeilToInt((float)hp / maxHp * 4f);

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
        base.Die();

        Debug.Log("🔥 BOSS DEFEATED 🔥");

        // TODO:
        // - открыть портал
        // - вызвать концовку
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
}
