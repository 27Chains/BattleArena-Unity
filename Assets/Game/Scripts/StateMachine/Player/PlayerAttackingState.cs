using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float transitionDuration = 0.1f;

    private string animationName = "Attack1";

    public PlayerAttackingState(PlayerStateMachine stateMachine) :
        base(stateMachine)
    {
    }

    public override void Enter()
    {
        Attack();
    }

    private void Attack()
    {
        stateMachine.Animator.CrossFadeInFixedTime (
            animationName,
            transitionDuration
        );
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
        Move((float) stateMachine.TimeManager.TickDelta);
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator, animationName) >= 1f)
        {
            stateMachine.SwitchState(new PlayerMovementState(stateMachine));
        }
    }
}
