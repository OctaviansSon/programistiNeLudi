using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        PlayerStats stats = col.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.ApplyItem(item);
            Destroy(gameObject);
        }
    }
}
