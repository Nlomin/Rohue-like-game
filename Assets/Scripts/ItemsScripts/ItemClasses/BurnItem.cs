using UnityEngine;

[CreateAssetMenu(fileName = "NewBurnItem", menuName = "Items/BurnEffect")]
public class BurnItem : Item
{
    public BurnEffect burnEffect;
    public float fireWaveChance = 0.2f; // 20%
    public float waveCooldown = 5f;

    public override void ApplyEffect(PlayerController player)
    {
        player.stats.AddEffect(burnEffect);
        player.stats.EnableFireWave(burnEffect, fireWaveChance, waveCooldown);
        Debug.Log("Игрок теперь может поджигать врагов и выпускать огненную волну");
    }
}

