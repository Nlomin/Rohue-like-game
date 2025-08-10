using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MEnemyAttack : MonoBehaviour, IAttack
{
    Animator anim;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public float attackRadius = 1.32f;
    [Range(1, 50)]
    public int damage = 1;
    private PlayerController player;
    private PlayerStats playerHealth;
    NavMeshAgent agent;
    public Collider swordCollider;

    public GameObject attackIndicatorPrefab;
    public Transform indicatorSpawnPoint;
    public TrailRenderer trail;
    private void Awake()
    {
        player = FindAnyObjectByType<PlayerController>();
        trail.emitting = false;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        playerHealth = player.GetComponent<PlayerStats>();   
        agent = GetComponent<NavMeshAgent>();

    }
    public virtual bool IsPlayerInAttackRange()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, attackRadius, playerLayer);

        if (collider.Length > 0)
        {
            anim.SetBool("isAttack", true);
            agent.isStopped = true;
            Vector3 direction = player.transform.position - transform.position;
            // �������� ������� �������, ����� ���� ������� � ������� ������
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // ������ ������������ �����
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
            return true;

        }
        else
        {
            
            anim.SetBool("isAttack", false);
            agent.isStopped = false;
            if (swordCollider.enabled == true)
                DeactivateSwordCollider();
            return false;
        }
    }
    public virtual void Attack()
    {
        playerHealth.TakeDamage(damage);
    }
    public void LateUpdate()
    {
        //AttackAnimation();
    }
    public void ActivateSwordCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
            trail.emitting = true;
        }
    }

    // ����������, ����� ��������� ��������� ����� �����
    public void DeactivateSwordCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
            trail.emitting = false;

        }
    }
    public void ShowAttackIndicator()
    {
        StartCoroutine(AttackTelegraphRoutine());
    }
    private IEnumerator AttackTelegraphRoutine()
    {
        GameObject indicator = Instantiate(attackIndicatorPrefab, indicatorSpawnPoint.position, Quaternion.identity);
        
        SpriteRenderer sr = indicator.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogWarning("� ���������� ��� SpriteRenderer!");
            yield break;
        }
        indicator.transform.SetParent(indicatorSpawnPoint);
        float duration = 0.25f;
        float elapsed = 0f;

        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;

        Color startColor = sr.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // ��������� ����������

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // �������
            indicator.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            // ������������
            sr.color = Color.Lerp(startColor, endColor, t);

            yield return null;
        }

        Destroy(indicator);

        yield return new WaitForSeconds(0.1f); // ��������� ����� ����� ������

        
    }



}
