using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour, IDamageable , IStatusEffectTarget
{
    [Header("Здоровье")]
    [Range(1, 100)]
    [SerializeField] int maxHP;
    public int currentHP;

    [Header("Атака")]
    [SerializeField] private int playerDamage = 10; // Базовый урон игрока

    [Header("Скорость атаки")]
    [SerializeField] private float attackSpeedMultiplier = 1f; 
    

    public UnityEvent onHealing;
    public UnityEvent onTakeDamage;
    public UnityEvent onDeath;
    Animator anim;

    private List<StatusEffect> availableEffects = new List<StatusEffect>();

    [Header("Эффект Горения")]
    
    private float fireWaveChance = 0f;
    private float fireWaveCooldown = 5f;
    private float lastWaveTime = -999f;


    [SerializeField] private GameObject burningWavePrefab;
    [SerializeField] private Transform waveSpawnPoint; // Например, перед мечом
    [SerializeField] private BurnEffect burnEffectWave;

    private Dictionary<StatusEffect, Coroutine> activeEffects = new Dictionary<StatusEffect, Coroutine>();
    [SerializeField] private GameObject effectIconPrefab;
    [SerializeField] private Transform iconParent;


    private void Start()
    {
        currentHP = maxHP;
        anim = GetComponent<Animator>();
    }
    public void Die()
    {
        anim.SetTrigger("dead");
        RunManager.Instance.StartNewRun();
    }
    
    public void RestoreHealth(int healing)
    {
        currentHP += healing;

        if (currentHP > maxHP) currentHP = maxHP;

        onHealing?.Invoke();
    }

    

    public void TakeDamage(int damage)
    {
        if (currentHP == 0) return;

        currentHP -= damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            onDeath?.Invoke();
        }
        else
            onTakeDamage?.Invoke();
    }
    public void TakeDamageTest()
    {
        Debug.Log("Pain sound");
        Debug.Log("Interface");
    }
    public int GetCurrentHp()
    {
        return currentHP;
    }



    public int GetDamage()
    {
        return playerDamage;
    }

    public void IncreaseDamage(int amount)
    {
        playerDamage += amount;
        Debug.Log($"Урон игрока увеличен! Новый урон: {playerDamage}");
    }

    public void SetDamage(int amount)
    {
        playerDamage = amount;
        Debug.Log($"Урон игрока установлен: {playerDamage}");
    }


    public void IncreaseAttackSpeed(float multiplier)
    {
        attackSpeedMultiplier *= multiplier;
        UpdateAttackSpeed();
        Debug.Log($"Скорость атаки увеличена! Новый множитель: {attackSpeedMultiplier}");
    }

    public void SetAttackSpeed(float multiplier)
    {
        attackSpeedMultiplier = multiplier;
        UpdateAttackSpeed();
        Debug.Log($"Скорость атаки установлена! Новый множитель: {attackSpeedMultiplier}");
    }

    private void UpdateAttackSpeed()
    {
        if (anim != null)
        {
            anim.SetFloat("AttackSpeedMultiplier", attackSpeedMultiplier);
        }
    }

    public void TryApplyEffectsToEnemy(MEnemyStats enemyStats)
    {
        if (enemyStats == null) return;

        IStatusEffectTarget target = enemyStats.GetComponent<IStatusEffectTarget>();
        if (target == null) return;

        foreach (StatusEffect effect in availableEffects)
        {
            effect.ApplyEffect(target);
        }
    }


    public void AddEffect(StatusEffect effect)
    {
        if (!availableEffects.Contains(effect))
        {
            availableEffects.Add(effect);
        }
    }
    public void EnableFireWave(BurnEffect effect, float chance, float cooldown)
    {
        burnEffectWave = effect;
        fireWaveChance = chance;
        fireWaveCooldown = cooldown;
    }
    private void TryTriggerFireWave()
    {
        if (!CanCastFireWave()) return;

        lastWaveTime = Time.time;
        SpawnFireSlash();
    }

    private bool CanCastFireWave()
    {
        return burnEffectWave != null &&
               Time.time >= lastWaveTime + fireWaveCooldown &&
               UnityEngine.Random.value <= fireWaveChance;
    }

    private void SpawnFireSlash()
    {
        GameObject wave = Instantiate(burningWavePrefab, waveSpawnPoint.position, transform.rotation);
        BurningWave burning = wave.GetComponent<BurningWave>();
        if (burning != null)
        {
            burning.burnEffect = burnEffectWave;
        }
    }


    public void RegisterEffect(StatusEffect effect, Coroutine routine)
    {
        if (!activeEffects.ContainsKey(effect))
            activeEffects.Add(effect, routine);
    }

    public void UnregisterEffect(StatusEffect effect)
    {
        if (activeEffects.ContainsKey(effect))
        {
            activeEffects.Remove(effect);
        }
    }

    public bool HasEffect(StatusEffect effect)
    {
        return activeEffects.ContainsKey(effect);
    }

    public void ShowEffectIcon(Sprite iconSprite, float lifetime)
    {
        if (effectIconPrefab == null || iconSprite == null || iconParent == null) return;

        GameObject iconGO = Instantiate(effectIconPrefab, iconParent);
        var img = iconGO.GetComponent<UnityEngine.UI.Image>();
        if (img != null)
        {
            img.sprite = iconSprite;
        }

        Destroy(iconGO, lifetime);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
