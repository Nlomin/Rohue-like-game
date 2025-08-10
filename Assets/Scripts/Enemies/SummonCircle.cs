using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SummonCircle : MonoBehaviour
{
    public Image fillImage;

    public IEnumerator Fill(float duration)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            fillImage.fillAmount = t;
            yield return null;
        }
    }
}
