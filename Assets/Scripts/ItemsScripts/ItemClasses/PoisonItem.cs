using UnityEngine;

[CreateAssetMenu(fileName = "NewPoisonItem", menuName = "Items/PoisonEffect")]
public class PoisonItem : Item
{
    public StatusEffect poisonEffect;

    public override void ApplyEffect(PlayerController player)
    {
        player.stats.AddEffect(poisonEffect);
        Debug.Log("Игрок теперь может отравлять врагов");
    }
}

