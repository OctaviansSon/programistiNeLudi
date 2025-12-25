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
        // Input создаём в Awake — но камеру не кэшируем тут
        input = new PlayerInputActions();
    }

    void Start()
    {
        // Камеру берём в Start — тогда при перезагрузке сцены ссылка будет валидной
        cam = Camera.main;
    }

    void OnEnable()
    {
        if (input == null) input = new PlayerInputActions();
        input.Enable();
        input.Gameplay.Fire.performed += OnFire;
    }

    void OnDisable()
    {
        if (input != null)
            input.Gameplay.Fire.performed -= OnFire;

        if (input != null)
            input.Disable();
    }

    void OnDestroy()
    {
        // на всякий случай
        if (input != null)
        {
            input.Gameplay.Fire.performed -= OnFire;
            input.Disable();
            input = null;
        }
    }

    void OnFire(InputAction.CallbackContext ctx)
    {
        if (Time.time - lastShoot < cooldown) return;

        lastShoot = Time.time;
        Shoot();
    }

    void Shoot()
    {
        if (cam == null)
            cam = Camera.main;
        if (cam == null) return; // если всё ещё нет камеры — выходим

        Vector2 mouseScreen = input.Gameplay.Look.ReadValue<Vector2>();
        // Используем z = 0 для 2D (orthographic). После конвертации сбрасываем z.
        Vector3 mouseWorld = cam.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, 0f));
        mouseWorld.z = 0f;

        Vector2 dir = (mouseWorld - transform.position);
        dir.Normalize();

        GameObject p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        var proj = p.GetComponent<Projectile>();
        if (proj != null) proj.Init(dir);
    }
}
