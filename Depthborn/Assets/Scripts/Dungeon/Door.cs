using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;

    public Collider2D triggerCol; // IsTrigger = true
    public Collider2D blockCol;   // IsTrigger = false
    public SpriteRenderer sprite;

    public enum DoorSide { Up, Down, Left, Right }
    public DoorSide side;

    public DungeonRoom leadsTo;   // Комната, в которую ведёт

    // debounce, чтобы одно нажатие не давало несколько вызовов
    float lastTriggerTime = -1f;
    const float triggerCooldown = 0.25f;

    void Awake()
    {
        // попытаемся автоматом найти коллайдеры, если не назначены
        if (triggerCol == null || blockCol == null)
        {
            Collider2D[] cols = GetComponents<Collider2D>();
            foreach (var c in cols)
            {
                if (c == null) continue;
                if (c.isTrigger) triggerCol = c;
                else blockCol = c;
            }
        }

        if (sprite == null) sprite = GetComponent<SpriteRenderer>();

        // корректная инициализация коллайдеров/вида
        InitializeColliders();
    }

    // вызывается генератором, чтобы гарантировать что коллайдеры настроены
    public void InitializeColliders()
    {
        if (blockCol == null || triggerCol == null)
        {
            Collider2D[] cols = GetComponents<Collider2D>();
            foreach (var c in cols)
            {
                if (c == null) continue;
                if (c.isTrigger) triggerCol = c;
                else blockCol = c;
            }
        }

        // если дверь помечена открытой — откроем корректно, иначе закроем
        if (isOpen) Open();
        else Close();
    }

    public void Open()
    {
        isOpen = true;
        if (blockCol != null) blockCol.enabled = false;
        if (triggerCol != null) triggerCol.enabled = true;
        if (sprite != null) sprite.color = Color.white;
    }

    public void Close()
    {
        isOpen = false;
        if (blockCol != null) blockCol.enabled = true;
        // важно: когда дверь закрыта, отключаем trigger, чтобы игрок не оказался в overlap'е и не вызвал телепорт
        if (triggerCol != null) triggerCol.enabled = false;
        if (sprite != null) sprite.color = Color.red;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpen) return;
        if (Time.time - lastTriggerTime < triggerCooldown) return;
        if (!other.CompareTag("Player")) return;

        lastTriggerTime = Time.time;

        if (leadsTo == null)
        {
            Debug.LogWarning($"Door {name} has no leadsTo assigned!");
            return;
        }

        RoomTransition rt = FindFirstObjectByType<RoomTransition>();
        if (rt == null)
        {
            Debug.LogError("Door: RoomTransition not found in scene!");
            return;
        }

        // передаём сторону, с которой выходим из текущей комнаты
        rt.EnterRoom(leadsTo, side);
    }
}
