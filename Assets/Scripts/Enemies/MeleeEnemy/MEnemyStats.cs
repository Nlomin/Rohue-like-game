using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MEnemyStats : MonoBehaviour, IDamageable, IStatusEffectTarget
{
    public RoomController roomOwner;
    [Header("’арактеристики")]
    public int maxHealth;
    public int currentHealth { get; private set; }
    public UnityEvent<int> OnDamageTaken;
    public UnityEvent OnDeath;
    private bool isDead = false;

    // Ёффекты
    private Dictionary<StatusEffect, Coroutine> activeEffects = new Dictionary<StatusEffect, Coroutine>();
    [SerializeField] private GameObject effectIconPrefab;
    [SerializeField] private Transform iconParent;

    public GameObject fracturedSkeletonPrefab;
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnDamageTaken?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        OnDeath.Invoke();
        if(fracturedSkeletonPrefab != null)
        {
            GameObject destroyedEnemy = Instantiate(fracturedSkeletonPrefab, transform.position, transform.rotation);
            Destroy(destroyedEnemy, 10f);
        }
        Destroy(gameObject);
    }

    public void RestoreHealth(int healing)
    {
        throw new System.NotImplementedException();
    }

    // –еализаци€ IStatusEffectTarget:
    public void RegisterEffect(StatusEffect effect, Coroutine routine)
    {
        if (!activeEffects.ContainsKey(effect))
            activeEffects.Add(effect, routine);
    }

    public void UnregisterEffect(StatusEffect effect)
    {
        if (activeEffects.ContainsKey(effect))
            activeEffects.Remove(effect);
    }

    public bool HasEffect(StatusEffect effect)
    {
        return activeEffects.ContainsKey(effect);
    }

    public void ShowEffectIcon(Sprite iconSprite, float lifetime)
    {
        if (effectIconPrefab == null || iconSprite == null || iconParent == null) return;

        GameObject iconGO = Instantiate(effectIconPrefab, iconParent);
        Image iconImage = iconGO.GetComponent<Image>();
        if (iconImage != null)
        {
            iconImage.sprite = iconSprite;
        }

        Destroy(iconGO, lifetime);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}

