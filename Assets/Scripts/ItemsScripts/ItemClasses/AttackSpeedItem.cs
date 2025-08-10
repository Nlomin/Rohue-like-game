using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackSpeedUpItem", menuName = "Items/AttackSpeedUp")]

public class AttackSpeedUp : Item
{
    [Header("��������� ��� ���������� �����")]
    public float attackSpeedIncrease = 1.5f;

    public override void ApplyEffect(PlayerController player)
    {
        // �����������, ��� � ������� Player ���� �����, ������������� ����:
        player.stats.IncreaseAttackSpeed(attackSpeedIncrease);

        // ����� ������� �����-�� ���������� � ������� ��� ������� �������:
        Debug.Log($"{itemName} ������� ����: +{attackSpeedIncrease}");
    }
}
