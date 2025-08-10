using UnityEngine;

[CreateAssetMenu(fileName = "NewFreezeItem", menuName = "Items/FreezeEffect")]
public class FreezeItem : Item
{
    [Header("Effect Reference")]
    public StatusEffect freezeEffect;  // —сылка на наш ScriptableObject

    public override void ApplyEffect(PlayerController player)
    {
        // например, мы даЄм игроку возможность примен€ть заморозку:
        player.stats.AddEffect(freezeEffect);
        Debug.Log("»грок получил возможность накладывать заморозку!");
    }
}
