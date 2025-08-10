using System.Collections;
using UnityEngine;

public interface IEnemyState
{
    void Enter();
    void Execute();
    void Exit();
}

public class EnemyIdleState : IEnemyState
{
    private EnemyController enemy;
    private Animator animator;

    public EnemyIdleState(EnemyController enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
    }

    public void Enter()
    {
        Debug.Log("Враг вошел в состояние ожидания.");
        animator.SetBool("isAttack", false);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        Debug.Log("Враг выходит из состояния ожидания.");
    }
}

public class EnemyPatrolState : IEnemyState
{
    private EnemyController enemy;
    private Animator animator;

    public EnemyPatrolState(EnemyController enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
    }

    public void Enter()
    {
        Debug.Log("Враг начинает патрулирование.");
        animator.SetBool("isAttack", false);
        animator.SetBool("isWalking", true);
    }

    public void Execute()
    {
        enemy.enemyPatrol.Patrol();

        if (enemy.enemyPatrol.CanSeePlayer(out Transform player))
        {
            Debug.Log("Игрок обнаружен! Начинаем преследование.");
            enemy.StateMachine.ChangeState(new EnemyChaseState(enemy, animator, player));
        }
    }

    public void Exit()
    {
        Debug.Log("Враг покидает режим патрулирования.");
    }
}


public class EnemyChaseState : IEnemyState
{
    private EnemyController enemy;
    private Animator animator;
    private Transform player;

    public EnemyChaseState(EnemyController enemy, Animator animator, Transform player)
    {
        this.enemy = enemy;
        this.animator = animator;
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Начинаем преследование игрока!");
        animator.SetBool("isAttack", false);
        animator.SetBool("isWalking", true);
    }

    public void Execute()
    {
        if (enemy.enemyAttack.IsPlayerInAttackRange())
        {
            enemy.StateMachine.ChangeState(new EnemyAttackState(enemy, animator));
        }
        else if(!enemy.enemyAttack.IsPlayerInAttackRange() && enemy.enemyPatrol.CanSeePlayer(out Transform player))
        {
            enemy.enemyPatrol.MoveTo(player.position);
        }
        else
        {
            enemy.StateMachine.ChangeState(new EnemyPatrolState(enemy, animator));
        }
    }

    public void Exit()
    {
        Debug.Log("Враг прекратил преследование.");
    }
}

public class EnemyAttackState : IEnemyState
{
    private EnemyController enemy;
    private Animator animator;
    private float attackCooldown = 1.5f; // Кулдаун перед следующей атакой
    private float lastAttackTime;
    private bool isAttacking = false; // Флаг атаки

    public EnemyAttackState(EnemyController enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
    }

    public void Enter()
    {
        if (!isAttacking)
        {
            isAttacking = true; // Устанавливаем флаг, чтобы не спамить атаками
            animator.SetBool("isAttack", true);
            lastAttackTime = Time.time;
            Debug.Log("Враг атакует!");
            animator.SetBool("isWalking", false);
        }
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Поворот к игроку во время атаки
        if (enemy.player != null)
        {
            Vector3 direction = enemy.player.transform.position - enemy.transform.position;
            direction.y = 0; // Чтобы враг не наклонялся
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, 5 * Time.deltaTime);
        }

        // Проверяем, закончилась ли анимация атаки и прошел ли кулдаун
        if (isAttacking && stateInfo.normalizedTime >= 1f && Time.time > lastAttackTime + attackCooldown)
        {
            isAttacking = false; // Сбрасываем флаг атаки
            animator.SetBool("isAttack", false);
            Debug.Log("Враг выходит из состояния атаки.");

            if (enemy.enemyAttack.IsPlayerInAttackRange())
            {
                enemy.StateMachine.ChangeState(new EnemyAttackState(enemy, animator)); // Повторная атака
            }
            else if (enemy.enemyPatrol.CanSeePlayer(out Transform player))
            {
                enemy.StateMachine.ChangeState(new EnemyChaseState(enemy, animator, player)); // Преследование
            }
            else
            {
                enemy.StateMachine.ChangeState(new EnemyPatrolState(enemy, animator)); // Патрулирование
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Враг выходит из состояния атаки.");
    }
}

public class EnemyFrozenState : IEnemyState
{
    private EnemyController enemy;
    private Animator animator;
    private float freezeDuration;

    public EnemyFrozenState(EnemyController enemy, Animator animator, float duration)
    {
        this.enemy = enemy;
        this.animator = animator;
        this.freezeDuration = duration;
    }

    public void Enter()
    {
        Debug.Log(" Враг заморожен!");
        animator.speed = 0; 
        

        enemy.StartCoroutine(UnfreezeAfterDelay(freezeDuration));
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        Debug.Log(" Враг разморожен!");
        animator.speed = 1;
        enemy.UnfreezeEnemy();
    }

    private IEnumerator UnfreezeAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        enemy.StateMachine.ChangeState(new EnemyPatrolState(enemy, animator)); 
    }
}

public class EnemySpawnState : IEnemyState
{
    private EnemyController enemy;
    private Animator animator;
    private float spawnAnimationDuration = 0.9f; // продолжительность анимации

    public EnemySpawnState(EnemyController enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
    }

    public void Enter()
    {
        Debug.Log("Враг появился, проигрывается анимация подъёма.");
        animator.SetTrigger("spawn"); // у тебя должна быть триггер-анимация
        enemy.StartCoroutine(WaitAndTransition());
        animator.SetBool("isWalking", false);
       
    }

    public void Execute() { }

    public void Exit()
    {
        Debug.Log("Анимация появления завершена, враг готов.");
    }

    private IEnumerator WaitAndTransition()
    {
        yield return new WaitForSeconds(spawnAnimationDuration);
        enemy.StateMachine.ChangeState(new EnemyPatrolState(enemy, animator));
    }
}
