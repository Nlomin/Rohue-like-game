using System.Linq;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public float interactDistance = 3f;
    public float cancelDialogueDistance = 6f;
    public KeyCode interactKey = KeyCode.E;

    private IInteractable nearestInteractable;
    private DialogueSystem dialogueSystem;

    private void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
    }

    private void Update()
    {
        FindNearestInteractable();

        if (nearestInteractable != null)
        {
            // Ќе показываем иконку, если диалог уже активен
            if (!dialogueSystem.isDialogueActive)
                nearestInteractable.ShowInteractIcon(interactKey.ToString());

            if (Input.GetKeyDown(interactKey))
            {
                if (!dialogueSystem.isDialogueActive)
                {
                    nearestInteractable.Interact();
                    nearestInteractable.HideInteractIcon();
                }
                else
                {
                    dialogueSystem.AdvanceOrSkip();
                }
            }
            if (ShelfInteractable.IsInspectingShelf)
            {
                nearestInteractable.HideInteractIcon();
            }

        }


        // —крыть все подсказки, если ничего р€дом
        if (nearestInteractable == null)
        {
            foreach (var obj in FindObjectsOfType<MonoBehaviour>())
            {
                if (obj is IInteractable interactable)
                {
                    interactable.HideInteractIcon();
                }
            }
        }

        // ѕрерывание диалога по рассто€нию
        if (dialogueSystem.isDialogueActive && nearestInteractable != null)
        {
            float dist = Vector3.Distance(transform.position, nearestInteractable.GetTransform().position);
            if (dist > cancelDialogueDistance)
            {
                dialogueSystem.EndDialogue();
            }
        }
    }

    void FindNearestInteractable()
    {
        IInteractable[] all = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().ToArray();
        float closestDist = interactDistance;
        nearestInteractable = null;

        foreach (var obj in all)
        {
            float dist = Vector3.Distance(transform.position, obj.GetTransform().position);
            if (dist <= closestDist)
            {
                closestDist = dist;
                nearestInteractable = obj;
            }
        }
    }
}
