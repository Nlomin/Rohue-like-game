using UnityEngine;
using UnityEngine.Events;

public class WeaponHitBox : MonoBehaviour
{
    private PlayerStats player;
    public UnityEvent OnHitEnemy;

    public GameObject slashEffectPrefab;
    public Transform slashSpawnPoint; // ����� ��������� slash-� (����� ���� ����� � �������)
    private void Start()
    {
        player = FindFirstObjectByType<PlayerStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") &&
            other.TryGetComponent(out MEnemyStats enemy))
        {
            // ������� ����
            enemy.TakeDamage(player.GetDamage());

            // ��������� ��� ������� ������ � �����
            player.TryApplyEffectsToEnemy(enemy);
            SpawnSlashEffect(other.ClosestPoint(transform.position));
            OnHitEnemy?.Invoke();
        }
        if (other.CompareTag("Barrel"))
        {
            other.GetComponent<BreakableObject>().Break();
            
        }
    }


    private void SpawnSlashEffect(Vector3 position)
    {
        GameObject slash = Instantiate(slashEffectPrefab, position, Quaternion.identity);

        // �������� �� ����������� ����� (�����������)
        Vector3 dir = (position - transform.position).normalized;
        if (dir != Vector3.zero)
            slash.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir); // ��� 2D �������

        Destroy(slash, 0.05f);
    }
}
