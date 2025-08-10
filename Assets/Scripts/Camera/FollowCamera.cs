using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;          // Игрок
    public Vector3 offset = new Vector3(0, 10, -10); // Смещение камеры
    public float smoothSpeed = 5f;    // Скорость следования

    void LateUpdate()
    {
        if (target == null) return;

        // Целевая позиция камеры
        Vector3 desiredPosition = target.position + offset;

        // Плавное движение
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Камера всегда смотрит на игрока (опционально)
        // transform.LookAt(target);
    }
}

