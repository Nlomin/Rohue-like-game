using UnityEngine;

[CreateAssetMenu(fileName = "NewStatusEffect", menuName = "Effects/Status Effect")]
public abstract class StatusEffect : ScriptableObject
{
    public float duration;
    public float chance;
    public Sprite icon;

    public GameObject particlePrefab; // частички
    public Vector3 particleOffset = Vector3.up; // куда их спавнить
    public virtual void ApplyEffect(IStatusEffectTarget target)
    {
        // Базовая реализация
    }

    
}

