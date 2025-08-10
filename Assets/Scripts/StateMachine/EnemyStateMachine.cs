using System;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    private IEnemyState currentState;
    public event Action<IEnemyState> stateChanged;

    public void Initialize(IEnemyState initialState)
    {
        currentState = initialState;
        currentState.Enter();
        stateChanged?.Invoke(initialState);
    }

    public void Execute()
    {
        currentState?.Execute();
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
        stateChanged?.Invoke(newState);
    }
}
