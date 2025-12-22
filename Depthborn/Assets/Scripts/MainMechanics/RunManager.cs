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
}
