using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewFreezeEffect", menuName = "Effects/Freeze Effect")]
public class FreezeEffect : StatusEffect
{
    public override void ApplyEffect(IStatusEffectTarget target)
    {
        if (target == null || target.HasEffect(this)) return;

        if (Random.value <= chance)
        {
            Coroutine routine = target.GetTransform().GetComponent<MonoBehaviour>().StartCoroutine(FreezeRoutine(target));
            target.RegisterEffect(this, routine);
        }
    }

    private IEnumerator FreezeRoutine(IStatusEffectTarget target)
    {
        GameObject particle = null;
        if (particlePrefab != null)
        {
            particle = Instantiate(particlePrefab, target.GetTransform().position, Quaternion.identity, target.GetTransform());
        }

        target.ShowEffectIcon(icon, duration);

        var freezable = target.GetTransform().GetComponent<IFreezable>();
        if (freezable != null)
        {
            freezable.FreezeEnemy(duration);
            Debug.Log($"FreezeEffect: Заморожен на {duration} секунд!");
        }

        yield return new WaitForSeconds(duration);

        if (particle != null) Destroy(particle);
        target.UnregisterEffect(this);
    }
}
