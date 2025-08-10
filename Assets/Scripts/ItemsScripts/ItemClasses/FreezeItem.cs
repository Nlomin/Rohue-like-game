using UnityEngine;

[CreateAssetMenu(fileName = "NewFreezeItem", menuName = "Items/FreezeEffect")]
public class FreezeItem : Item
{
    [Header("Effect Reference")]
    public StatusEffect freezeEffect;  // ������ �� ��� ScriptableObject

    public override void ApplyEffect(PlayerController player)
    {
        // ��������, �� ��� ������ ����������� ��������� ���������:
        player.stats.AddEffect(freezeEffect);
        Debug.Log("����� ������� ����������� ����������� ���������!");
    }
}
