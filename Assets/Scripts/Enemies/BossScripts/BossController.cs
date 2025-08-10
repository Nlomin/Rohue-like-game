using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


public class BossController : MonoBehaviour, IFreezable
{
    public BossStateMachine StateMachine { get; private set; }
    public Animator animator;

    [Header("Поведение")]
    public float decisionDelay = 5f; // Пауза перед следующей попыткой выбрать атаку
    private float decisionTimer;

    [Header("Ссылки на поведение")]
    public BossSummoner summoner;
    public SphereRotator sphereRotator;

    public MEnemyStats bossStats { get; private set; }

    private List<BossAttackOption> attacks = new List<BossAttackOption>();
    private BossAttackOption currentAttack;
    public BossProjectileLauncher projectileLauncher;

    public Transform player;

    [Header("Сближение")]
    public float maxChaseDistance = 15f; // Если игрок дальше — сближаемся
    public float approachRange = 6f;     // Если ближе — хватит

    [Header("Отступление")]
    public float retreatCooldown = 6f;
    private float lastRetreatTime = -999f;

    [Header("UI")]

    //HealthBars
    [SerializeField] private EnemyHealthBar healthBar;
    [SerializeField] private Transform healthBarAnchor;
    [SerializeField] private GameObject healthBarPrefab;

    private Dictionary<StatusEffect, Coroutine> activeEffects = new Dictionary<StatusEffect, Coroutine>();
    public bool isFrozen = false;

    [Header("Границы")]
    public BoxCollider arenaBounds;
    public NavMeshAgent agent;

    public GameObject winPanelPrefab;
    public UnityEvent OnBossDefeat;
    
    private void Awake()
    {
        StateMachine = new BossStateMachine();
        summoner = GetComponent<BossSummoner>();
        projectileLauncher = GetComponent<BossProjectileLauncher>();
        bossStats = GetComponent<MEnemyStats>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        // Начальное состояние — Idle
        StateMachine.Initialize(new BossIdleState(this));
        decisionTimer = decisionDelay;
        player = FindFirstObjectByType<PlayerController>().GetComponent<Transform>();
        // Добавляем атаки
        attacks.Add(new BossAttackOption
        {
            Name = "Summon",
            Cooldown = 30f,
            LastUsedTime = -999f,
            CreateState = () => new BossSummonState(this)
        });
        attacks.Add(new BossAttackOption
        {
            Name = "Projectile",
            Cooldown = 12f,
            LastUsedTime = -999f,
            CreateState = () => new BossProjectileAttackState(this)
        });

        if (healthBarPrefab != null && healthBarAnchor != null)
        {
            GameObject hb = Instantiate(healthBarPrefab, healthBarAnchor.position, Quaternion.identity, healthBarAnchor);
            healthBar = hb.GetComponent<EnemyHealthBar>();
        }

        // Подписка на урон
        if (bossStats != null)
        {
            bossStats.OnDamageTaken.AddListener(UpdateHealthBar);
        }

    }



    private void Update()
    {
        StateMachine.Execute();

        // FSM сама контролирует переходы, но решение об атаке — здесь
        if (StateMachine.currentState is BossIdleState)
        {
            decisionTimer -= Time.deltaTime;

            if (decisionTimer <= 0f)
            {
                BossAttackOption availableAttack = GetAvailableAttack();
                if (availableAttack != null)
                {
                    currentAttack = availableAttack;
                    StateMachine.ChangeState(currentAttack.CreateState());
                }

                decisionTimer = decisionDelay; // обновляем таймер независимо от результата
            }
        }
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
            }
        }
        
    }
   

    

    private BossAttackOption GetAvailableAttack()
    {
        foreach (var attack in attacks)
        {
            if (attack.IsReady)
                return attack;
        }
        return null;
    }
    public bool HasAvailableAttack()
    {
        foreach (var attack in attacks)
        {
            if (attack.IsReady)
                return true;
        }
        return false;
    }


    public void CompleteAttack()
    {
        if (currentAttack != null)
        {
            currentAttack.LastUsedTime = Time.time;
            currentAttack = null;
        }

        StateMachine.ChangeState(new BossIdleState(this));
    }

    public bool CanRetreat()
    {
        return Time.time >= lastRetreatTime + retreatCooldown;
    }

    public void SetRetreatUsed()
    {
        lastRetreatTime = Time.time;
    }

    public void ApplyEffect(StatusEffect effect)
    {
        if (effect == null) return;
        effect.ApplyEffect(bossStats);
    }
    public bool HasEffect(StatusEffect effect)
    {
        return activeEffects.ContainsKey(effect);
    }
    public void RegisterEffect(StatusEffect effect, Coroutine routine)
    {
        if (!activeEffects.ContainsKey(effect))
        {
            activeEffects.Add(effect, routine);
        }
    }

    public void UnregisterEffect(StatusEffect effect)
    {
        if (activeEffects.ContainsKey(effect))
        {
            activeEffects.Remove(effect);
        }
    }
    public void FreezeEnemy(float freezeDuration)
    {
        if (isFrozen) return;

        isFrozen = true;

        StateMachine.ChangeState(new BossFrozenState(this, animator, freezeDuration));
    }
    public void ResumeMovement()
    {
        isFrozen = false;
    }


    private void UpdateHealthBar(int currentHealth)
    {
        // Можно обновить UI шкалы здоровья
    }
    public Vector3 ClampPositionToBounds(Vector3 position, CapsuleCollider bounds)
    {
        Bounds b = bounds.bounds;

        position.x = Mathf.Clamp(position.x, b.min.x, b.max.x);
        position.y = transform.position.y; // Чтобы не падал/не взлетал
        position.z = Mathf.Clamp(position.z, b.min.z, b.max.z);

        return position;
    }
    private void OnDestroy()
    {
        // Проверяем, что объект уничтожается в "нормальном" режиме
        if (this.gameObject.activeSelf)
        {
            OnBossDefeat.Invoke();
            
        }
    }

}


public class BossAttackOption
{
    public string Name;
    public float Cooldown;
    public float LastUsedTime;
    public Func<IBossState> CreateState;

    public bool IsReady => Time.time >= LastUsedTime + Cooldown;
}

