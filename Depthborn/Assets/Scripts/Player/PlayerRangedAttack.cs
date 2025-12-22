using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRangedAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float cooldown = 0.25f;

    float lastShoot;
    PlayerInputActions input;
    Camera cam;

    void Awake()
    {
        input = new PlayerInputActions();
        cam = Camera.main;
    }

    void OnEnable()
    {
        input.Enable();
        input.Gameplay.Fire.performed += OnFire;
    }

    void OnDisable()
    {
        input.Gameplay.Fire.performed -= OnFire;
        input.Disable();
    }

    void OnFire(InputAction.CallbackContext ctx)
    {
        if (Time.time - lastShoot < cooldown) return;

        lastShoot = Time.time;
        Shoot();
    }

    void Shoot()
    {
        Vector2 mouseScreen = input.Gameplay.Look.ReadValue<Vector2>();
        Vector3 mouseWorld = cam.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, cam.nearClipPlane));
        mouseWorld.z = 0f;

        Vector2 dir = (mouseWorld - transform.position);
        dir.Normalize();

        GameObject p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        var proj = p.GetComponent<Projectile>();
        if (proj != null) proj.Init(dir);
    }
}
