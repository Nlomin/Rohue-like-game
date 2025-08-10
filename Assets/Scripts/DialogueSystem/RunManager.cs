using UnityEngine;
using System;
using System.Collections.Generic;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }

    public int CurrentRun { get; private set; } = 0;

    public event Action OnNewRunStarted;

    // ������ ������ ������� ��� ������� NPC
    private Dictionary<string, int> npcDialogueIndices = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartNewRun()
    {
        
        CurrentRun++;
        Debug.Log($"����� �����: {CurrentRun}");
        OnNewRunStarted?.Invoke();

    }

    public void ResetRuns()
    {
        CurrentRun = 0;
        npcDialogueIndices.Clear();
    }

    public int GetDialogueIndex(string npcId)
    {
        return npcDialogueIndices.TryGetValue(npcId, out var index) ? index : 0;
    }

    public void SetDialogueIndex(string npcId, int index)
    {
        npcDialogueIndices[npcId] = index;
    }
}

