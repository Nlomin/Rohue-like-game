using UnityEngine;
using TMPro;


public class ShelfInteractable : MonoBehaviour, IInteractable
{
    [Header("UI взаимодействи€")]
    public GameObject interactIcon;
    public TextMeshProUGUI keyLabel;

    [Header("—сылка на камеру дл€ приближени€")]
    public Transform inspectCameraTarget;
    public float cameraMoveSpeed = 3f;

    private CameraScript cameraController;
    //private bool isInspecting = false;

    public static bool IsInspectingShelf { get; private set; } = false;
    private PlayerController playerController;

    public BoxCollider ShopRoomCollider;
    private void Start()
    {
        cameraController = FindFirstObjectByType<CameraScript>(); 
        playerController = FindFirstObjectByType<PlayerController>();
        
    }

    public void Interact()
    {
        if (!IsInspectingShelf)
        {
            StartInspecting();
        }
        else
        {
            StopInspecting();
        }
    }

    private void StartInspecting()
    {
        IsInspectingShelf = true;
        
        playerController?.SetInputLocked(IsInspectingShelf);

        cameraController.MoveToTarget(inspectCameraTarget.position, inspectCameraTarget.rotation, cameraMoveSpeed);
        ShopRoomCollider.enabled = false;
    }

    private void StopInspecting()
    {
        IsInspectingShelf = false;

        playerController?.SetInputLocked(IsInspectingShelf);

        cameraController.ReturnToPlayer(cameraMoveSpeed);
        ShopRoomCollider.enabled = true;
    }

    public void ShowInteractIcon(string keyText)
    {
        keyLabel.text = keyText;
        interactIcon.SetActive(true);
    }

    public void HideInteractIcon()
    {
        interactIcon.SetActive(false);
    }

    public Transform GetTransform()
    {
        return transform;
    }
    
}

