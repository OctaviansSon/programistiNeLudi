using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 pos = target.position;
        pos.z = -10;     // ВСЕГДА -10
        transform.position = Vector3.Lerp(transform.position, pos, speed * Time.deltaTime);
    }
}
