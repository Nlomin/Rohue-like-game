using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBurnEffect", menuName = "Effects/Burn Effect")]
public class BurnEffect : StatusEffect
{
    [Header("Burn Settings")]
    public int burnDamage = 1;
    public float tickInterval = 1f;

    public override void ApplyEffect(IStatusEffectTarget target)
    {
        if (target == null || target.HasEffect(this)) return;

        if (Random.value <= chance)
        {
            Coroutine routine = target.GetTransform().GetComponent<MonoBehaviour>().StartCoroutine(BurnRoutine(target));
            target.RegisterEffect(this, routine);
        }
    }

    private IEnumerator BurnRoutine(IStatusEffectTarget target)
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
            IDamageable damageable = target.GetTransform().GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(burnDamage);
            }

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }

        if (particle != null) Destroy(particle);
        target.UnregisterEffect(this);
    }

}

