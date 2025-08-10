using UnityEngine;
public class TorchFlicker : MonoBehaviour
{
    public Light torchLight;
    public float baseIntensity = 3f;
    public float flickerAmount = 0.5f;
    public float flickerSpeed = 5f;

    void Update()
    {
        if (torchLight != null)
        {
            float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0.0f);
            torchLight.intensity = baseIntensity + (noise - 0.5f) * flickerAmount;
        }
    }
}

