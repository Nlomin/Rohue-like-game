using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;
public interface IBossState
{
    void Enter();
    void Execute();
    void Exit();
}

public class BossIdleState : IBossState
{
    private BossController boss;
    private Transform player;

    private float wanderInterval = 2f;
    private float wanderTimer = 0f;
    private float wanderRadius = 5f;

    public BossIdleState(BossController boss)
    {
        this.boss = boss;
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
    }

    public void Enter()
    {
        boss.animator?.SetBool("isIdle", true);
        wanderTimer = wanderInterval;
        Debug.Log("���� � ��������� Idle.");
    }

    public void Execute()
    {
        if (player == null) return;

        float distance = Vector3.Distance(boss.transform.position, player.position);

        if (distance < 4f && boss.CanRetreat())
        {
            boss.StateMachine.ChangeState(new BossRetreatState(boss));
            return;
        }
        else if (distance > boss.maxChaseDistance)
        {
            boss.StateMachine.ChangeState(new BossApproachState(boss));
            return;
        }

        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0f)
        {
            TryWander();
            wanderTimer = wanderInterval;
        }
    }

    public void Exit()
    {
        boss.animator?.SetBool("isIdle", false);
    }

    private void TryWander()
    {
        Vector3 randomDir = Random.insideUnitSphere * wanderRadius;
        randomDir += boss.transform.position;
        randomDir.y = boss.transform.position.y;

        if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            boss.agent.SetDestination(hit.position);
            Debug.Log("���� ������: " + hit.position);
        }
    }
}




public class BossSummonState : IBossState
{
    private BossController boss;

    public BossSummonState(BossController boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        Debug.Log("���� ����� ������!");
        boss.animator?.SetTrigger("SummonEnemies");

        boss.CompleteAttack(); 
    }

    public void Execute() { }

    public void Exit()
    {
        Debug.Log("���� �������� ������.");
    }
}

public class BossProjectileAttackState : IBossState
{
    private BossController boss;
    private float attackDuration = 5f;
    private float launchTimer;
    private float interval;
    private float elapsed;

    private Transform player;

    private float startDelay = 2f; // �������� ����� ������ ��������
    private bool hasStartedLaunching = false;

    public BossProjectileAttackState(BossController boss)
    {
        this.boss = boss;
        this.interval = boss.projectileLauncher.launchInterval;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    public void Enter()
    {
        Debug.Log("���� ����� ����� ���������!");
        elapsed = 0f;
        launchTimer = 0f;
        hasStartedLaunching = false;

        boss.animator?.SetTrigger("AttackSpheres");
        boss.animator.SetBool("isIdle", false);
    }

    public void Execute()
    {
        if (player == null)
        {
            boss.CompleteAttack();
            return;
        }

        elapsed += Time.deltaTime;

        if (!hasStartedLaunching)
        {
            if (elapsed >= startDelay)
            {
                hasStartedLaunching = true;
                launchTimer = 0f; // ���������� ������ ��� ������� ������
            }
            return; // ��� ���� �� ������ ��������
        }

        launchTimer += Time.deltaTime;

        if (launchTimer >= interval)
        {
            boss.projectileLauncher.LaunchProjectilesTowards(player.position);
            launchTimer = 0f;
        }

        if (elapsed >= attackDuration)
        {
            boss.CompleteAttack();
        }
    }

    public void Exit()
    {
        Debug.Log("���� �������� ����� ���������.");
    }
}
public class BossRetreatState : IBossState
{
    private BossController boss;
    private Transform player;

    private float retreatDuration = 3f;
    private float timer;

    private float minDistance = 5f;
    private float maxDistance = 15f;
    private int maxAttempts = 10;

    public BossRetreatState(BossController boss)
    {
        this.boss = boss;
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
    }

    public void Enter()
    {
        boss.SetRetreatUsed();
        boss.agent.isStopped = false;
        timer = retreatDuration;
        Debug.Log("���� �������� ��������� �� ������.");

        Vector3 retreatDirection = (boss.transform.position - player.position).normalized;
        retreatDirection.y = 0f;

        bool found = false;
        for (int i = 0; i < maxAttempts; i++)
        {
            float angleOffset = Random.Range(-90f, 90f); // � ������� "�����-�����", �� �� ������ �����
            Vector3 dir = Quaternion.Euler(0, angleOffset, 0) * retreatDirection;
            float distance = Random.Range(minDistance, maxDistance);
            Vector3 target = boss.transform.position + dir.normalized * distance;

            if (NavMesh.SamplePosition(target, out NavMeshHit hit, 3f, NavMesh.AllAreas))
            {
                boss.agent.SetDestination(hit.position);
                Debug.Log($"���� ��������� � {hit.position}");
                found = true;
                break;
            }
        }

        if (!found)
        {
            Debug.LogWarning("���� �� ���� ����� ����� ��� �����������. ����� �� �����.");
            boss.agent.SetDestination(boss.transform.position);
        }
    }

    public void Execute()
    {
        if (boss.isFrozen) return;
        timer -= Time.deltaTime;

        bool reached = !boss.agent.pathPending && boss.agent.remainingDistance <= boss.agent.stoppingDistance;
        if (timer <= 0f || reached || boss.HasAvailableAttack())
        {
            boss.CompleteAttack();
        }
    }

    public void Exit()
    {
        boss.agent.ResetPath();
        Debug.Log("���� �������� �����������.");
    }
}




public class BossApproachState : IBossState
{
    private BossController boss;
    private Transform player;
    private float preferredDistance = 6f;

    public BossApproachState(BossController boss)
    {
        this.boss = boss;
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
    }

    public void Enter()
    {
        Debug.Log("���� �������� ������������ ����� NavMesh.");
        boss.agent.isStopped = false;
    }

    public void Execute()
    {
        if (player == null)
        {
            boss.CompleteAttack();
            return;
        }

        float distance = Vector3.Distance(boss.transform.position, player.position);

        if (distance <= preferredDistance || boss.HasAvailableAttack())
        {
            boss.agent.ResetPath();
            boss.CompleteAttack();
            return;
        }

        if (!boss.agent.pathPending && boss.agent.remainingDistance > preferredDistance)
        {
            boss.agent.SetDestination(player.position);
        }
    }

    public void Exit()
    {
        boss.agent.ResetPath();
        Debug.Log("���� �������� �����������.");
    }
}
public class BossFrozenState : IBossState
{
    private BossController boss;
    private Animator animator;
    private float freezeDuration;

    public BossFrozenState(BossController boss, Animator animator, float duration)
    {
        this.boss = boss;
        this.animator = animator;
        this.freezeDuration = duration;
    }

    public void Enter()
    {
        Debug.Log(" ���� ���������!");
        animator.speed = 0;


        boss.StartCoroutine(UnfreezeAfterDelay(freezeDuration));
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        Debug.Log(" ���� ����������!");
        animator.speed = 1;

    }

    private IEnumerator UnfreezeAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        boss.isFrozen = false;
        boss.StateMachine.ChangeState(new BossIdleState(boss));
    }
}




