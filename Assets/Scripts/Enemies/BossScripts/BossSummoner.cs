using UnityEngine;

public class BossSummoner : MonoBehaviour
{
    [Header("����������� �����")]
    public GameObject[] enemyPrefabs;

    [Header("��������� �������")]
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

        Debug.Log($"���� ������� {numberToSummon} ������");
    }

    private Vector3 GetRandomPointAroundBoss()
    {
        Vector2 circle = Random.insideUnitCircle * summonRadius;
        Vector3 position = transform.position + new Vector3(circle.x, 5f, circle.y); // ������ ���� ����

        // �������� ����� ��� ������
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 10f, groundMask))
        {
            return hit.point;
        }

        // fallback � ���� ��� �����
        return transform.position + Vector3.forward * 2f;
    }
    

}

