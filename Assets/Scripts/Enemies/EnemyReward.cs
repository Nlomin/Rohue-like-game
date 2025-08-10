using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    [Header("������� � ������")]
    public int minGold = 1;
    public int maxGold = 5;

    [Header("������ ������������ ������")]
    public GameObject floatingGoldTextPrefab;

    public void GiveReward()
    {
        int reward = Random.Range(minGold, maxGold + 1);
        CurrencyManager.Instance.AddGold(reward);

        if (floatingGoldTextPrefab != null)
        {
            GameObject textObj = Instantiate(
                floatingGoldTextPrefab,
                transform.position + Vector3.up * 2f, // ������� ���� �����
                Quaternion.identity
            );

            FloatingGoldText floatingText = textObj.GetComponent<FloatingGoldText>();
            floatingText?.SetGoldAmount(reward);
        }

        
    }
}
