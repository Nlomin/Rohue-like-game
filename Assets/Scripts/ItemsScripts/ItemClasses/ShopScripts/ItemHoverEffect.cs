using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ItemPickUp))]
public class ItemHoverEffect : MonoBehaviour
{
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f);
    public float scaleSpeed = 5f;

    private Vector3 originalScale;
    private bool isHovered = false;
    private ItemPickUp itemPickUp;
    public float showDistance = 2f;
    
    

    private void Start()
    {
        originalScale = transform.localScale;
        itemPickUp = GetComponent<ItemPickUp>();
       
       
    }

    private void Update()
    {
        if (!ShelfInteractable.IsInspectingShelf && transform.localScale != originalScale)
        {
            isHovered = false;
            ItemTooltipUI.Instance?.Hide();
        }

        Vector3 targetScale = isHovered ? hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);

        
    }


    private void OnMouseEnter()
    {
        if (!ShelfInteractable.IsInspectingShelf) return;

        isHovered = true;

        if (itemPickUp.item != null)
        {
            ItemTooltipUI.Instance?.Show(itemPickUp.item, transform);
        }
    }

    private void OnMouseExit()
    {
        isHovered = false;
        ItemTooltipUI.Instance?.Hide();
    }
    private void OnMouseDown()
    {
        // Только если игрок в режиме осмотра
        if (!ShelfInteractable.IsInspectingShelf) return;

        // Получаем игрока
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player == null || itemPickUp?.item == null) return;

        Item item = itemPickUp.item;
        int price = item.price;
        // Применяем эффект
        if (CurrencyManager.Instance.TrySpendGold(price))
        {
            item.ApplyEffect(player);

            ItemTooltipUI.Instance?.Hide();
            if (itemPickUp.floatingGoldTextPrefab != null)
            {
                GameObject textObj = Instantiate(
                    itemPickUp.floatingGoldTextPrefab,
                    transform.position,
                    Quaternion.identity
                );

                FloatingGoldText floatingText = textObj.GetComponent<FloatingGoldText>();
                floatingText?.SetText($"-{price}", Color.yellow);
            }
            // Удаляем предмет с полки
            Destroy(gameObject);   
        }
        else
        {
            
            if (itemPickUp.floatingGoldTextPrefab != null)
            {
                GameObject textObj = Instantiate(
                    itemPickUp.floatingGoldTextPrefab,
                    transform.position + Vector3.back * 2f + Vector3.left * 2f,
                    Quaternion.identity
                );

                FloatingGoldText floatingText = textObj.GetComponent<FloatingGoldText>();
                floatingText?.SetText("Недостаточно золота", Color.red);
            }

        }
    }
}



