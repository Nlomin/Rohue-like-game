using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public static event Action<int> OnGoldChanged;

    [Header("Настройки валюты")]
    public int gold { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadCurrency();
        ResetGold();
        OnGoldChanged?.Invoke(gold); // сразу обновим UI при загрузке
    }

    public void AddGold(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold);
        SaveCurrency();
    }

    public bool TrySpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            OnGoldChanged?.Invoke(gold);
            SaveCurrency();
            return true;
        }

        Debug.Log("Недостаточно золота!");
        return false;
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.Save();
    }

    private void LoadCurrency()
    {
        gold = PlayerPrefs.GetInt("Gold", 0);
    }

    public void ResetGold()
    {
        gold = 0;
        OnGoldChanged?.Invoke(gold);
        SaveCurrency();
    }
}


