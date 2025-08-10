using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RangeAttackEnemy : MonoBehaviour, IAttack
{
    Animator anim;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackRadius = 7f;
    
    public PlayerController player;
    private PlayerStats playerHealth;
    NavMeshAgent agent;
    public GameObject arrowPrefab; // Префаб стрелы
    public Transform shootPoint;  // Точка выстрела (например, конец лука)

    public GameObject attackIndicatorPrefab;
    public Transform indicatorSpawnPoint;
    private void Awake()
    {
        player = FindAnyObjectByType<PlayerController>();
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
            // Поворот врага в сторону игрока
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
            return true;
        }
        else
        {
            anim.SetBool("isAttack", false);
            agent.isStopped = false;
            return false;
        }
    }

    public void Attack()
    {
        if (player == null || arrowPrefab == null || shootPoint == null) return;

        
        Vector3 target = player.transform.position + Vector3.up * 1.5f;

       
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);

       
        ArrowScript arrowScript = arrow.GetComponent<ArrowScript>();
        if (arrowScript != null)
        {
            arrowScript.SetDirection(target);
        }

       
        Quaternion lookRotation = Quaternion.LookRotation(target - shootPoint.position);

        
        Quaternion correction = Quaternion.Euler(180, 0f, 0f); 
        arrow.transform.rotation = lookRotation * correction;

        // (Опционально: визуальная отладка)
        Debug.DrawRay(shootPoint.position, (target - shootPoint.position).normalized * 3f, Color.red, 2f);
    }

    public void ShowAttackIndicator()
    {
        StartCoroutine(AttackTelegraphRoutine());
    }

    private IEnumerator AttackTelegraphRoutine()
    {
        GameObject indicator = Instantiate(attackIndicatorPrefab, indicatorSpawnPoint.position, Quaternion.identity);
        SpriteRenderer sr = indicator.GetComponent<SpriteRenderer>();
        indicator.transform.SetParent(indicatorSpawnPoint);
        if (sr == null)
        {
            Debug.LogWarning("У индикатора нет SpriteRenderer!");
            yield break;
        }

        float duration = 0.4f;
        float elapsed = 0f;

        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;

        Color startColor = sr.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // полностью прозрачный

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Масштаб
            indicator.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            // Прозрачность
            sr.color = Color.Lerp(startColor, endColor, t);

            yield return null;
        }

        Destroy(indicator);

        yield return new WaitForSeconds(0.1f); // небольшая пауза перед ударом

       
    }
    public void LateUpdate()
    {
        //AttackAnimation();
    }
}
