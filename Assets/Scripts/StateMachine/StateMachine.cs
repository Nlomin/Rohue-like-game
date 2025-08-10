
using System;

using UnityEngine;


public class StateMachine
{
    private IState currentState { get; set; }
    public WalkState walkState;
    public IdleState idleState;
    public AttackState attackState;
    public DashState dashState;

    public event Action<IState> stateChanged;

    public StateMachine(PlayerController playerController, Animator animator)
    {
        this.walkState = new WalkState(playerController, animator);

        this.idleState = new IdleState(playerController, animator);

        this.attackState = new AttackState(playerController, animator);
        this.dashState = new DashState(playerController, animator);
    }
    

    public void Initialize(IState state)
    {
        currentState = state;
        currentState.Enter();
        stateChanged?.Invoke(state);
    }

    public void Execute()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }

    public void ChangeState(IState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();

        // notify other objects that state has changed
        stateChanged?.Invoke(newState);
    }
}
