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

    void Awake()
    {
        if (triggerCol == null || blockCol == null)
        {
            Collider2D[] cols = GetComponents<Collider2D>();
            foreach (var c in cols)
            {
                if (c.isTrigger) triggerCol = c;
                else blockCol = c;
            }
        }

        if (sprite == null) sprite = GetComponent<SpriteRenderer>();

        // По умолчанию закрыта
        if (isOpen)
            Open();
        else
            Close();
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
        if (triggerCol != null) triggerCol.enabled = true; // триггер оставляем, чтобы ловил игрока
        if (sprite != null) sprite.color = Color.red;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpen) return;
        if (!other.CompareTag("Player")) return;

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

        // передаём сторону, с которой ВЫХОДИМ из текущей комнаты
        rt.EnterRoom(leadsTo, side);
    }
}
