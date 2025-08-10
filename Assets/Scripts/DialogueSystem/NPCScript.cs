using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCscript : MonoBehaviour, IInteractable
{
    public string npcId; // ”никальный ID (задать в инспекторе)
    private DialogueSystem dialogueSystem;
    public GameObject interactIcon;
    public TextMeshProUGUI keyLabel;

    public List<TextAsset> runDialogues;
    public TextAsset repeatDialogue;

    private bool hasTalkedThisRun = false;
    public DialogueBubble dialogueBubble;
    private PlayerController playerController;
    private void Awake()
    {
        if (dialogueSystem == null)
            dialogueSystem = FindObjectOfType<DialogueSystem>();
        playerController = FindFirstObjectByType<PlayerController>();
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(npcId))
        {
            npcId = gameObject.name; // fallback
        }

        RunManager.Instance.OnNewRunStarted += OnNewRunStarted;
    }

    private void OnDestroy()
    {
        if (RunManager.Instance != null)
            RunManager.Instance.OnNewRunStarted -= OnNewRunStarted;
    }

    private void OnNewRunStarted()
    {
        if (hasTalkedThisRun)
        {
            int currentIndex = RunManager.Instance.GetDialogueIndex(npcId);
            RunManager.Instance.SetDialogueIndex(npcId, currentIndex + 1);
        }

        hasTalkedThisRun = false;
    }

    public void Interact()
    {
        if (dialogueSystem == null || runDialogues.Count == 0) return;

        if (hasTalkedThisRun)
        {
            if (repeatDialogue != null)
            {
                dialogueSystem.LoadDialogue(repeatDialogue);
                
            }    
                
        }
        else
        {
            int index = Mathf.Clamp(RunManager.Instance.GetDialogueIndex(npcId), 0, runDialogues.Count - 1);
            dialogueSystem.LoadDialogue(runDialogues[index]);
            hasTalkedThisRun = true;
            
        }
        
    }

    public void ShowInteractIcon(string keyText)
    {
        if (interactIcon != null)
        {
            keyLabel.text = keyText;
            interactIcon.SetActive(true);
        }
    }

    public void HideInteractIcon()
    {
        if (interactIcon != null)
        {
            interactIcon.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}



