using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    [Header("Награда в золоте")]
    public int minGold = 1;
    public int maxGold = 5;

    [Header("Префаб всплывающего текста")]
    public GameObject floatingGoldTextPrefab;

    public void GiveReward()
    {
        int reward = Random.Range(minGold, maxGold + 1);
        CurrencyManager.Instance.AddGold(reward);

        if (floatingGoldTextPrefab != null)
        {
            GameObject textObj = Instantiate(
                floatingGoldTextPrefab,
                transform.position + Vector3.up * 2f, // немного выше врага
                Quaternion.identity
            );

            FloatingGoldText floatingText = textObj.GetComponent<FloatingGoldText>();
            floatingText?.SetGoldAmount(reward);
        }

        
    }
}
