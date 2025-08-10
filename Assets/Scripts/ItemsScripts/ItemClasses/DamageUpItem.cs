using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageUpItem", menuName = "Items/DamageUp")]

public class DamageUpItem : Item
{
    [Header("��������� ��� ���������� �����")]
    public int damageIncrease = 20;

    public override void ApplyEffect(PlayerController player)
    {
        // �����������, ��� � ������� Player ���� �����, ������������� ����:
        player.stats.IncreaseDamage(damageIncrease);

        // ����� ������� �����-�� ���������� � ������� ��� ������� �������:
        Debug.Log($"{itemName} ������� ����: +{damageIncrease}");
    }
}
