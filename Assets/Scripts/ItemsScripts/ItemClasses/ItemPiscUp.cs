using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [Header("ScriptableObject с данными о предмете")]
    public Item item;
    public GameObject floatingGoldTextPrefab;

    private void OnTriggerEnter(Collider other)
    {
        // Допустим, что у игрока тег Player
        if (other.CompareTag("Player"))
        {
            // Получаем скрипт игрока
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Применяем эффект
                item.ApplyEffect(player);

                // Уничтожаем предмет, чтобы нельзя было взять его повторно
                Destroy(gameObject);
            }
        }
    }
}
