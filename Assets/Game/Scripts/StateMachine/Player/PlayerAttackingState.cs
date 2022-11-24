using FishNet;
using UnityEngine;
using UnityEngine.InputSystem;

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
        if (InstanceFinder.IsClient && stateMachine.IsOwner)
        {
            Vector3 mousePosition = GetMousePositionInWorld();
            Vector3 direction =
                (mousePosition - stateMachine.transform.position).normalized;
            stateMachine.ServerRotateAttackDirection (direction);
        }

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

    private Vector3 GetMousePositionInWorld()
    {
        LayerMask layerMask = LayerMask.GetMask("Ground");

        Ray ray =
            Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, layerMask))
        {
            return hit.point;
        }

        return Vector3.zero;
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
