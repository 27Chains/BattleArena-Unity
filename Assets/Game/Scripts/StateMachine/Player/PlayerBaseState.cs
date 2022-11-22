using FishNet.Object.Prediction;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    [Replicate]
    protected void Move(
        MoveData moveData,
        bool asServer,
        bool replaying = false
    )
    {
        float deltaTime = (float) stateMachine.TimeManager.TickDelta;
        Vector3 movement = (moveData.Movement);

        stateMachine.CharacterController.Move(movement * deltaTime);
    }

    [Reconcile]
    protected void Reconcile(ReconcileData recData, bool asServer)
    {
        stateMachine.transform.position = recData.Position;
    }

    protected void ReturnToLocomotion()
    {
        stateMachine.SwitchState(new PlayerMovementState(stateMachine));
    }
}
