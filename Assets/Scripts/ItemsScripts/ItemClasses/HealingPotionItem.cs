using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageUpItem", menuName = "Items/HealingPotion")]

public class HealingItem : Item
{
    [Header("���������� ��������")]
    public int healing = 20;

    public override void ApplyEffect(PlayerController player)
    {
        // �����������, ��� � ������� Player ���� �����, ������������� ����:
        player.stats.RestoreHealth(healing);
    }
}