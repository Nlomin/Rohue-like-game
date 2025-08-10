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
        Debug.Log("���� ����� � ��������� ��������.");
        animator.SetBool("isAttack", false);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        Debug.Log("���� ������� �� ��������� ��������.");
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
        Debug.Log("���� �������� ��������������.");
        animator.SetBool("isAttack", false);
        animator.SetBool("isWalking", true);
    }

    public void Execute()
    {
        enemy.enemyPatrol.Patrol();

        if (enemy.enemyPatrol.CanSeePlayer(out Transform player))
        {
            Debug.Log("����� ���������! �������� �������������.");
            enemy.StateMachine.ChangeState(new EnemyChaseState(enemy, animator, player));
        }
    }

    public void Exit()
    {
        Debug.Log("���� �������� ����� ��������������.");
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
        Debug.Log("�������� ������������� ������!");
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
        Debug.Log("���� ��������� �������������.");
    }
}

public class EnemyAttackState : IEnemyState
{
    private EnemyController enemy;
    private Animator animator;
    private float attackCooldown = 1.5f; // ������� ����� ��������� ������
    private float lastAttackTime;
    private bool isAttacking = false; // ���� �����

    public EnemyAttackState(EnemyController enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
    }

    public void Enter()
    {
        if (!isAttacking)
        {
            isAttacking = true; // ������������� ����, ����� �� ������� �������
            animator.SetBool("isAttack", true);
            lastAttackTime = Time.time;
            Debug.Log("���� �������!");
            animator.SetBool("isWalking", false);
        }
    }

    public void Execute()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // ������� � ������ �� ����� �����
        if (enemy.player != null)
        {
            Vector3 direction = enemy.player.transform.position - enemy.transform.position;
            direction.y = 0; // ����� ���� �� ����������
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, 5 * Time.deltaTime);
        }

        // ���������, ����������� �� �������� ����� � ������ �� �������
        if (isAttacking && stateInfo.normalizedTime >= 1f && Time.time > lastAttackTime + attackCooldown)
        {
            isAttacking = false; // ���������� ���� �����
            animator.SetBool("isAttack", false);
            Debug.Log("���� ������� �� ��������� �����.");

            if (enemy.enemyAttack.IsPlayerInAttackRange())
            {
                enemy.StateMachine.ChangeState(new EnemyAttackState(enemy, animator)); // ��������� �����
            }
            else if (enemy.enemyPatrol.CanSeePlayer(out Transform player))
            {
                enemy.StateMachine.ChangeState(new EnemyChaseState(enemy, animator, player)); // �������������
            }
            else
            {
                enemy.StateMachine.ChangeState(new EnemyPatrolState(enemy, animator)); // ��������������
            }
        }
    }

    public void Exit()
    {
        Debug.Log("���� ������� �� ��������� �����.");
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
        Debug.Log(" ���� ���������!");
        animator.speed = 0; 
        

        enemy.StartCoroutine(UnfreezeAfterDelay(freezeDuration));
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        Debug.Log(" ���� ����������!");
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
    private float spawnAnimationDuration = 0.9f; // ����������������� ��������

    public EnemySpawnState(EnemyController enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
    }

    public void Enter()
    {
        Debug.Log("���� ��������, ������������� �������� �������.");
        animator.SetTrigger("spawn"); // � ���� ������ ���� �������-��������
        enemy.StartCoroutine(WaitAndTransition());
        animator.SetBool("isWalking", false);
       
    }

    public void Execute() { }

    public void Exit()
    {
        Debug.Log("�������� ��������� ���������, ���� �����.");
    }

    private IEnumerator WaitAndTransition()
    {
        yield return new WaitForSeconds(spawnAnimationDuration);
        enemy.StateMachine.ChangeState(new EnemyPatrolState(enemy, animator));
    }
}
