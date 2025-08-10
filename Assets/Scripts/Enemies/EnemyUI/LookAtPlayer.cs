using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;

        // Поворачиваем объект к камере
        transform.forward = mainCamera.transform.forward;
    }
}
