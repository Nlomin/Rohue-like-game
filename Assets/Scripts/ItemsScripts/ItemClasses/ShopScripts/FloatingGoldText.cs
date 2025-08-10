using UnityEngine;
using TMPro;

public class FloatingGoldText : MonoBehaviour
{
    public TMP_Text goldText;
    public float floatSpeed = 1f;
    public float lifetime = 1.5f;

    private Vector3 moveDirection = Vector3.up;

    public void SetText(string text, Color color)
    {
        goldText.text = text;
        goldText.color = color;
    }

    public void SetGoldAmount(int amount)
    {
        SetText($"+{amount}", Color.yellow);
    }

    void Update()
    {
        transform.position += moveDirection * floatSpeed * Time.deltaTime;
        transform.LookAt(Camera.main.transform);
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}

