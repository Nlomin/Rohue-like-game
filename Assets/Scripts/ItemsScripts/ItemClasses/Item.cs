using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("����� ���������� � ��������")]
    public string itemName;
    public Sprite icon;
    [TextArea]
    public string description;

    [Header("���� � ������")]
    public int price = 10;
    public abstract void ApplyEffect(PlayerController player);
}
