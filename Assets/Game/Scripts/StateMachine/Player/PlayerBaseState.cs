using FishNet.Object.Prediction;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void ReturnToLocomotion()
    {
        stateMachine.SwitchState(new PlayerMovementState(stateMachine));
    }
}
