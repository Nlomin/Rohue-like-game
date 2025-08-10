using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IFreezable
{
    public EnemyStateMachine StateMachine { get; private set; }
    public MEnemyMovement enemyMovement { get; private set; }
    public MEnemyStats enemyStats { get; private set; }
    public IAttack enemyAttack { get; private set; }
    public IChase enemyPatrol { get; private set; }

    private Animator animator;
    private NavMeshAgent agent;
    private bool isFrozen = false;

    public PlayerController player;

    [SerializeField] private EnemyHealthBar healthBar;
    [SerializeField] private Transform healthBarAnchor;
    [SerializeField] private GameObject healthBarPrefab;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        enemyAttack = GetComponent<IAttack>();
        enemyPatrol = GetComponent<IChase>();
        enemyStats = GetComponent<MEnemyStats>();

        player = FindAnyObjectByType<PlayerController>();
        StateMachine = new EnemyStateMachine();
    }

    private void Start()
    {
        // Создание хелсбара
        if (healthBarPrefab != null && healthBarAnchor != null)
        {
            GameObject hb = Instantiate(healthBarPrefab, healthBarAnchor.position, Quaternion.identity, healthBarAnchor);
            healthBar = hb.GetComponent<EnemyHealthBar>();
        }

        // Подписка на урон
        if (enemyStats != null)
        {
            enemyStats.OnDamageTaken.AddListener(UpdateHealthBar);
        }

        // Начальное состояние
        //StateMachine.Initialize(new EnemyPatrolState(this, animator));
        StateMachine.Initialize(new EnemySpawnState(this, animator));

    }

    private void Update()
    {
        StateMachine.Execute();
    }

    public void ResumeMovement()
    {
        isFrozen = false;
        agent.isStopped = false;
    }

    public void FreezeEnemy(float freezeDuration)
    {
        if (isFrozen) return;

        isFrozen = true;
        StateMachine.ChangeState(new EnemyFrozenState(this, animator, freezeDuration));
    }
    public void UnfreezeEnemy()
    {
        if (isFrozen)
            isFrozen = false;
        else
            return;
    }
    private void UpdateHealthBar(int currentHealth)
    {
        // Можно обновить UI шкалы здоровья
    }
}

