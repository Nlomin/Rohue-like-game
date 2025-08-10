using UnityEngine;

public class Floating : MonoBehaviour
{
    public float amplitude = 0.2f;     // ������ �����
    public float frequency = 1f;       // ������� ���������

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPos + new Vector3(0, offsetY, 0);
    }
}

