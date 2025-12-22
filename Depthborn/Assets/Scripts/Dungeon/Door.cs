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

    float lastTriggerTime = -1f;
    const float triggerCooldown = 0.25f;

    void Awake()
    {
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

        InitializeColliders();
    }

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

        rt.EnterRoom(leadsTo, side);
    }
}
