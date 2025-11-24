using UnityEngine;

public class DamageTester : MonoBehaviour
{
    PlayerHealth hp;

    void Start()
    {
        hp = FindFirstObjectByType<PlayerHealth>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            hp.TakeDamage(1);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            hp.Heal(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            hp.AddShield(1);
    }
}
  
