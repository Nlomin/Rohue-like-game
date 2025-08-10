using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI itemNameText;

    private Item currentItem;

    public void SetItem(Item item)
    {
        currentItem = item;
        iconImage.sprite = item.icon;
        itemNameText.text = item.itemName;
    }

    public void OnClick()
    {
        Debug.Log($"Нажат предмет: {currentItem.itemName}");
       
    }
}


