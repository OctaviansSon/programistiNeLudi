using UnityEngine;

public class ExitPortal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        FindFirstObjectByType<EndingMenu>()?.Show();
    }
}
