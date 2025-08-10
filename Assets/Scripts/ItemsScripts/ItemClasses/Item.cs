using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("Общая информация о предмете")]
    public string itemName;
    public Sprite icon;
    [TextArea]
    public string description;

    [Header("Цена в Золоте")]
    public int price = 10;
    public abstract void ApplyEffect(PlayerController player);
}
