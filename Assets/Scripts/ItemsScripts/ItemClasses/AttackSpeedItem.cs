using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackSpeedUpItem", menuName = "Items/AttackSpeedUp")]

public class AttackSpeedUp : Item
{
    [Header("Настройки для увеличения урона")]
    public float attackSpeedIncrease = 1.5f;

    public override void ApplyEffect(PlayerController player)
    {
        // Предположим, что в скрипте Player есть метод, увеличивающий урон:
        player.stats.IncreaseAttackSpeed(attackSpeedIncrease);

        // Можно вывести какую-то информацию в консоль или вызвать событие:
        Debug.Log($"{itemName} добавил урон: +{attackSpeedIncrease}");
    }
}
