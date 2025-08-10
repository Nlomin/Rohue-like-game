using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    public GameObject bubbleUI;
    public TextMeshProUGUI textField;
    public float typingSpeed = 0.025f;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string fullText;

    private void Awake()
    {
        bubbleUI.SetActive(false);
    }

    public void ShowLine(string text)
    {
        bubbleUI.SetActive(true);

        fullText = text;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(text));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        textField.text = "";

        foreach (char c in text)
        {
            textField.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void SkipTyping()
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            textField.text = fullText;
            isTyping = false;
        }
    }

    public bool IsTyping()
    {
        return isTyping;
    }

    public void Hide()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        bubbleUI.SetActive(false);
        textField.text = "";
        isTyping = false;
    }

    public bool IsVisible()
    {
        return bubbleUI.activeSelf;
    }
}

