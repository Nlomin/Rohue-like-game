using System;
using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    public IBossState currentState;
    public event Action<IBossState> onStateChanged;

    public void Initialize(IBossState initialState)
    {
        currentState = initialState;
        currentState.Enter();
        onStateChanged?.Invoke(initialState);
    }

    public void Execute()
    {
        currentState?.Execute();
    }

    public void ChangeState(IBossState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
        onStateChanged?.Invoke(newState);
    }
}
