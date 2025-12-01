using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;

    public Collider2D triggerCol;     // IsTrigger = true
    public Collider2D blockCol;       // IsTrigger = false
    public SpriteRenderer sprite;

    public DungeonRoom leadsTo;       // Комната, в которую ведет

    public void Open()
    {
        isOpen = true;
        blockCol.enabled = false;
        sprite.color = Color.white;
    }

    public void Close()
    {
        isOpen = false;
        blockCol.enabled = true;
        sprite.color = Color.red;
    }
}
