using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    private void OnEnable()
    {
        CurrencyManager.OnGoldChanged += UpdateGold;
    }

    private void OnDisable()
    {
        CurrencyManager.OnGoldChanged -= UpdateGold;
    }

    private void UpdateGold(int amount)
    {
        goldText.text = amount.ToString();
    }
}


