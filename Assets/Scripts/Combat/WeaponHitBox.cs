using UnityEngine;
using UnityEngine.Events;

public class WeaponHitBox : MonoBehaviour
{
    private PlayerStats player;
    public UnityEvent OnHitEnemy;

    public GameObject slashEffectPrefab;
    public Transform slashSpawnPoint; // точка появления slash-а (может быть рядом с лезвием)
    private void Start()
    {
        player = FindFirstObjectByType<PlayerStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") &&
            other.TryGetComponent(out MEnemyStats enemy))
        {
            // Наносим урон
            enemy.TakeDamage(player.GetDamage());

            // Применяем все эффекты игрока к врагу
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

        // Вращение по направлению удара (опционально)
        Vector3 dir = (position - transform.position).normalized;
        if (dir != Vector3.zero)
            slash.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir); // для 2D спрайта

        Destroy(slash, 0.05f);
    }
}
