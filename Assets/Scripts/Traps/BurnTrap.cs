using System.Collections;
using UnityEngine;

public class BurnTrap : MonoBehaviour
{
    [Header("Настройки активации")]
    public float interval = 5f;
    public float activeDuration = 3f;

    [Header("Урон")]
    public int damage = 5;

    [Header("Эффекты")]
    public BurnEffect burnEffect;

    [Header("Коллайдер (зона ловушки)")]
    public Collider triggerCollider; // должен быть триггером

    [Header("VFX")]
    public GameObject fireEffect;

    private void Start()
    {
        if (triggerCollider != null)
            triggerCollider.enabled = false;

        if (fireEffect != null)
            fireEffect.SetActive(false);

        StartCoroutine(TrapRoutine());
    }

    private IEnumerator TrapRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            ActivateTrap();

            yield return new WaitForSeconds(activeDuration);

            DeactivateTrap();
        }
    }

    private void ActivateTrap()
    {
        if (triggerCollider != null)
            triggerCollider.enabled = true;

        if (fireEffect != null)
            fireEffect.SetActive(true);

       
    }

    private void DeactivateTrap()
    {
        if (triggerCollider != null)
            triggerCollider.enabled = false;

        if (fireEffect != null)
            fireEffect.SetActive(false);

       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerCollider.enabled) return;

        Debug.Log($"Ловушка столкнулась с: {other.name}");

        var damageable = other.GetComponent<IDamageable>();
        var effectTarget = other.GetComponent<IStatusEffectTarget>();

        if (damageable != null)
        {
            Debug.Log($"Найден IDamageable: {other.name}");
            damageable.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning($"IDamageable не найден у {other.name}");
        }

        if (effectTarget != null && burnEffect != null)
        {
            Debug.Log($"Найден IStatusEffectTarget: {other.name}");
            burnEffect.ApplyEffect(effectTarget);
        }
    }


}

