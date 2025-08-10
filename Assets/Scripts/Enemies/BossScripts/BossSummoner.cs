using UnityEngine;

public class BossSummoner : MonoBehaviour
{
    [Header("Призываемые враги")]
    public GameObject[] enemyPrefabs;

    [Header("Настройки призыва")]
    public int numberToSummon = 3;
    public float summonRadius = 6f;
    public LayerMask groundMask;

    public void SummonEnemies()
    {
        for (int i = 0; i < numberToSummon; i++)
        {
            Vector3 randomPos = GetRandomPointAroundBoss();

            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            SummonWithEffect summonUtil = FindFirstObjectByType<SummonWithEffect>();
            StartCoroutine(summonUtil.Summon(randomPos, enemyPrefab));

        }

        Debug.Log($"Босс призвал {numberToSummon} врагов");
    }

    private Vector3 GetRandomPointAroundBoss()
    {
        Vector2 circle = Random.insideUnitCircle * summonRadius;
        Vector3 position = transform.position + new Vector3(circle.x, 5f, circle.y); // ставим чуть выше

        // Проверим землю под точкой
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 10f, groundMask))
        {
            return hit.point;
        }

        // fallback — если нет земли
        return transform.position + Vector3.forward * 2f;
    }
    

}

