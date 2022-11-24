using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float transitionDuration = 0.1f;

    private string animationName = "Attack1";

    private bool alreadyAppliedForce;

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
        float normalizedTime =
            GetNormalizedTime(stateMachine.Animator, animationName);

        if (normalizedTime < 1f)
        {
            if (normalizedTime >= 0.35f)
            {
                TryApplyForce();
            }
        }
        else
        {
            stateMachine.SwitchState(new PlayerMovementState(stateMachine));
        }
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce || !stateMachine.IsOwner)
        {
            return;
        }
        alreadyAppliedForce = true;
        stateMachine
            .ForceReceiver
            .AddForce(stateMachine.transform.forward * 5f);
    }
}
