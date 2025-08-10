using System.Collections;
using UnityEngine;


public class FlashEffect : MonoBehaviour
{
    public float fadeInTime = 0.1f;
    public float holdTime = 0.05f;
    public float fadeOutTime = 0.2f;

    public SpriteRenderer spriteRenderer;
    public ParticleSystem particles; // можно сделать необязательным

    private void Start()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        Color color = spriteRenderer.color;
        float alpha = 0f;

        // Fade In
        float t = 0f;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            alpha = Mathf.Lerp(0f, 1f, t / fadeInTime);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Peak
        spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
        //if (particles != null) particles.Play();
        yield return new WaitForSeconds(holdTime);

        // Fade Out
        t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            alpha = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}

