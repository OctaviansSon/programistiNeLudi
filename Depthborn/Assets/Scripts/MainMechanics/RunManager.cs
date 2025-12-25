using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    [Header("Progress")]
    public int floor = 1;

    [Header("Dungeon Scaling")]
    public int baseRooms = 5;
    public int roomsPerFloor = 1;

    [Header("Enemy Scaling")]
    public int enemyHPPerFloor = 2;
    public int bossHPPerFloor = 5;
    public float enemyDamageMultiplier = 1f;
    public float damageGrowthPerFloor = 0.1f;

    // ===== saved player state between floors =====
    [System.Serializable]
    public class SavedPlayerStats
    {
        public int maxHP;
        public int hp;
        public int shield;
        public float moveSpeed;
        public float damageMultiplier;
    }

    public SavedPlayerStats savedStats = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // вызывается при Continue
    public void NextFloor()
    {
        floor++;
        enemyDamageMultiplier += damageGrowthPerFloor;

        Debug.Log($"➡ FLOOR {floor}");
    }

    // сколько комнат генерить
    public int GetRoomCount()
    {
        return baseRooms + floor * roomsPerFloor;
    }

    // бонус HP обычным мобам
    public int EnemyHPBonus()
    {
        return floor * enemyHPPerFloor;
    }

    // бонус HP боссу
    public int BossHPBonus()
    {
        return floor * bossHPPerFloor;
    }

    // ---- saved stats helpers ----
    public void SavePlayerStats(PlayerHealth ph, PlayerStats ps)
    {
        if (ph == null || ps == null) return;
        savedStats = new SavedPlayerStats
        {
            maxHP = ph.maxHP,
            hp = ph.hp,
            shield = ph.shield,
            moveSpeed = ps.movement != null ? ps.movement.moveSpeed : 0f,
            damageMultiplier = ps.damageMultiplier
        };
    }

    public bool HasSavedStats() => savedStats != null;

    public void ApplySavedStats(PlayerHealth ph, PlayerStats ps)
    {
        if (savedStats == null || ph == null || ps == null) return;

        ph.maxHP = savedStats.maxHP;
        ph.hp = Mathf.Clamp(savedStats.hp, 0, ph.maxHP);
        ph.shield = Mathf.Clamp(savedStats.shield, 0, ph.maxShield);

        if (ps.movement != null) ps.movement.moveSpeed = savedStats.moveSpeed;
        ps.damageMultiplier = savedStats.damageMultiplier;

        // не сбрасываем savedStats — можно переиспользовать или очистить при желании
    }
}
