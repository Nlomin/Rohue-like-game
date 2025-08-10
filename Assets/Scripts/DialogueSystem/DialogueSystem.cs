using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Linq;


public delegate void DialogueAction();

public class DialogueSystem : MonoBehaviour
{
    public Camera cam;
    public GameObject dialogueRoot; // общий объект с DialogueBubbles
    public float displayTime = 2.5f;

    private CDialogue dialogue = new CDialogue();
    private Dictionary<string, DialogueAction> actions = new Dictionary<string, DialogueAction>();
    public bool isDialogueActive = false;
    private DialogueBubble currentBubble;

    private void Awake()
    {
        actions = new Dictionary<string, DialogueAction>();
        actions["end"] = EndDialogue;
        actions["none"] = null;
        
    }

    public void LoadDialogue(TextAsset xmlFile)
    {
        dialogue.Clear();
        actions.Clear();

        // Примеры действий 
        actions.Add("dialogue end", EndDialogue);
        actions.Add("none", null);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlFile.text);  

        foreach (XmlNode msgNode in xmlDoc.SelectNodes("//message"))
        {
            CMessage msg = new CMessage
            {
                msgID = long.Parse(msgNode.Attributes["uid"].Value),
                speakerId = msgNode.Attributes["speaker"].Value,
                action = msgNode.Attributes["action"].Value,
                text = msgNode.InnerText.Trim()
            };
            dialogue.AddMessage(msg);
        }

        ShowNextMessage();
        isDialogueActive = true;
    }


    public void ShowNextMessage()
    {
        if (!dialogue.HasMoreMessages())
        {
            EndDialogue();
            return;
        }

        CMessage msg = dialogue.GetCurrentMessage();
        Debug.Log($"[Диалог] speaker: {msg.speakerId}, text: {msg.text}");

        GameObject speaker = GameObject.Find(msg.speakerId);
        if (speaker != null)
        {
            var bubble = speaker.GetComponentInChildren<DialogueBubble>(true);
            if (bubble != null)
            {
                 
                HideAllDialogueBubbles();

                bubble.ShowLine(msg.text);
                currentBubble = bubble;
                bubble.ShowLine(msg.text);
            }
            else
            {
                Debug.LogWarning($"DialogueBubble не найден у объекта {speaker.name}");
            }
        }
        else
        {
            Debug.LogWarning($"Спикер '{msg.speakerId}' не найден в сцене.");
        }

        if (actions.ContainsKey(msg.action))
        {
            actions[msg.action]?.Invoke();
        }

        dialogue.Advance();
        

    }
    private void HideAllDialogueBubbles()
    {
        var allBubbles = FindObjectsByType<DialogueBubble>(FindObjectsSortMode.None);
        foreach (var bubble in allBubbles)
        {
            bubble.Hide();
        }
    }


    public void EndDialogue()
    {
        Debug.Log("Диалог завершён.");
        isDialogueActive = false;

        // Скрыть все баблы
        var allBubbles = FindObjectsByType<DialogueBubble>(FindObjectsSortMode.None);
        foreach (var bubble in allBubbles)
        {
            bubble.Hide();
        }

        // Скрыть все подсказки
        var allInteractables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IInteractable>();
        foreach (var interactable in allInteractables)
        {
            interactable.HideInteractIcon();
        }
    }




    public void SetAction(string name, DialogueAction act)
    {
        actions[name] = act;
    }

    public void AdvanceOrSkip()
    {
        if (currentBubble != null && currentBubble.IsTyping())
        {
            currentBubble.SkipTyping();
        }
        else
        {
            ShowNextMessage();
        }
    }

}



public class CMessage
{
    public long msgID = -1;
    public string text = "";
    public string speakerId = "";
    public string action = "";
}

public class CDialogue
{
    public List<CMessage> messages = new List<CMessage>();
    public int currentIndex = 0;

    public void Clear()
    {
        messages.Clear();
        currentIndex = 0;
    }

    public void AddMessage(CMessage msg)
    {
        messages.Add(msg);
    }

    public CMessage GetCurrentMessage()
    {
        if (currentIndex < messages.Count)
            return messages[currentIndex];
        return null;
    }

    public void Advance()
    {
        currentIndex++;
    }

    public bool HasMoreMessages()
    {
        return currentIndex < messages.Count;
    }
}

