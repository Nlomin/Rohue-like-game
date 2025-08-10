using UnityEngine;
using Unity.Cinemachine;

public class ShopCamera : MonoBehaviour
{
    public CinemachineCamera shopCamera;
    public Transform inspectCameraTarget;
    private CameraScript cameraController;
    
    public float cameraMoveSpeed = 3f;
    private void Start()
    {
        cameraController = FindFirstObjectByType<CameraScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraController.MoveToTarget(inspectCameraTarget.position, inspectCameraTarget.rotation, cameraMoveSpeed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraController.ReturnToPlayer(cameraMoveSpeed);
        }
    }
}

