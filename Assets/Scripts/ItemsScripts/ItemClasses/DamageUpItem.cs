using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageUpItem", menuName = "Items/DamageUp")]

public class DamageUpItem : Item
{
    [Header("Настройки для увеличения урона")]
    public int damageIncrease = 20;

    public override void ApplyEffect(PlayerController player)
    {
        // Предположим, что в скрипте Player есть метод, увеличивающий урон:
        player.stats.IncreaseDamage(damageIncrease);

        // Можно вывести какую-то информацию в консоль или вызвать событие:
        Debug.Log($"{itemName} добавил урон: +{damageIncrease}");
    }
}
