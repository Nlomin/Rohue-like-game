using UnityEngine;
using System.Collections;

public class UIShowFade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject GameOverPanel;
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;            // стартовая прозрачность
        canvasGroup.interactable = false;  // чтобы не взаимодействовать пока не появится
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowGameOver()
    {
        GameOverPanel.SetActive(true);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float time = 0f;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}

