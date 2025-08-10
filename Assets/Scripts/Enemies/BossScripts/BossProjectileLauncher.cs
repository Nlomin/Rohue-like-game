using UnityEngine;

public class BossProjectileLauncher : MonoBehaviour
{
    [Header("Настройки атаки снарядами")]
    public GameObject projectilePrefab;
    public Transform[] launchPoints;
    public float launchInterval = 0.3f; // как часто выстрел
    public float projectileSpeed = 8f;

    public void LaunchProjectilesTowards(Vector3 targetPosition)
    {
        foreach (var point in launchPoints)
        {
            Vector3 flatTarget = new Vector3(targetPosition.x, point.position.y, targetPosition.z);
            Vector3 direction = (flatTarget - point.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, point.position, Quaternion.identity);
            Destroy(projectile, 5f);
            var orbit = projectile.GetComponent<ProjectilesScript>();
            if (orbit != null)
            {
                orbit.SetDirection(direction, projectileSpeed);
            }
        }
    }
}

