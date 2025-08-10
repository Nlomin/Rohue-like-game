using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewPoisonEffect", menuName = "Effects/Poison Effect")]
public class PoisonEffect : StatusEffect
{
    [Header("Poison Settings")]
    public int damagePerTick = 2;
    public float tickInterval = 1f;

    public override void ApplyEffect(IStatusEffectTarget target)
    {
        if (target == null || target.HasEffect(this)) return;

        if (Random.value <= chance)
        {
            Coroutine routine = target.GetTransform().GetComponent<MonoBehaviour>().StartCoroutine(PoisonRoutine(target));
            target.RegisterEffect(this, routine);
        }
    }

    private IEnumerator PoisonRoutine(IStatusEffectTarget target)
    {
        GameObject particle = null;
        if (particlePrefab != null)
        {
            particle = Instantiate(particlePrefab, target.GetTransform().position + particleOffset, Quaternion.identity, target.GetTransform());
        }

        target.ShowEffectIcon(icon, duration);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            // Наносим урон через IDamageable, если цель его поддерживает
            IDamageable damageable = target.GetTransform().GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damagePerTick);
                Debug.Log($"PoisonEffect: Нанесён {damagePerTick} урон ядом");
            }

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }

        if (particle != null) Destroy(particle);
        target.UnregisterEffect(this);
        Debug.Log("PoisonEffect: Эффект завершён");
    }
}

