using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltipUI : MonoBehaviour
{
    public static ItemTooltipUI Instance;

    public GameObject panel;
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text priceText;
    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(Item item, Transform attachTo)
    {
        //iconImage.sprite = item.icon;
        nameText.text = item.itemName;
        descriptionText.text = item.description;
        priceText.text = item.price.ToString();

        transform.SetParent(attachTo.parent.parent);
        transform.localPosition = Vector3.up * 1.5f + Vector3.back * 1.2f; // можно настраивать позицию относительно объекта
        transform.localRotation = Quaternion.identity;

        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
        transform.SetParent(null); // отсоединяемся от предмета
        transform.localScale = Vector3.one;
    }

    private void LateUpdate()
    {
        if (panel.activeSelf)
        {
            // Всегда поворачиваемся к камере
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
}

