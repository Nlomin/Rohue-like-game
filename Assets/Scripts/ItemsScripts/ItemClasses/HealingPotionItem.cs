using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageUpItem", menuName = "Items/HealingPotion")]

public class HealingItem : Item
{
    [Header("Увеличение здоровья")]
    public int healing = 20;

    public override void ApplyEffect(PlayerController player)
    {
        // Предположим, что в скрипте Player есть метод, увеличивающий урон:
        player.stats.RestoreHealth(healing);
    }
}