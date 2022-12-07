using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    private int DodgeAnimHash = Animator.StringToHash("Dodge");

    private Vector3 dodgeDirection;

    private float dodgeDuration = 0.75f;

    public PlayerDodgeState(PlayerStateMachine stateMachine) :
        base(stateMachine)
    {
    }

    public override void Enter()
    {
        if (!stateMachine.IsOwner) return;
        stateMachine.Player.ServerPlayAnim (DodgeAnimHash, crossFadeDuration);

        Vector3 direction = stateMachine.InputReader.GetDirection();
        stateMachine.Player.ServerRotatePlayer (direction);
        stateMachine
            .Player
            .ServerApplyForce(direction,
            stateMachine.DodgeForce,
            ForceType.Linear,
            dodgeDuration);
    }

    public override void Exit()
    {
    }

    public override void MovementUpdate(
        MoveData moveData,
        bool asServer,
        bool replaying = false
    )
    {
        Move((float) stateMachine.Player.TimeManager.TickDelta);
    }

    public override void Tick(float deltaTime)
    {
        if (!stateMachine.IsOwner) return;
        if (GetNormalizedTime(stateMachine.Animator, "Dodge") >= 0.8f)
        {
            stateMachine.SwitchState(PlayerState.Movement);
        }
    }
}
