using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float cooldown = 0.25f;
    float lastShoot;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > lastShoot + cooldown)
        {
            lastShoot = Time.time;
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouse - transform.position).normalized;

        GameObject p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        p.GetComponent<Projectile>().Init(dir);
    }
}
