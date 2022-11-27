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
        stateMachine.CrossFadeAnimation (animationName, transitionDuration);
        Attack();
    }

    private void Attack()
    {
        Vector3 mousePosition = GetMousePositionInWorld();
        Vector3 direction =
            (mousePosition - stateMachine.transform.position).normalized;
        stateMachine.transform.rotation = Quaternion.LookRotation(direction);
    }

    public override void Exit()
    {
        alreadyAppliedForce = false;
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
            stateMachine.SwitchState(PlayerState.Movement);
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
        if (alreadyAppliedForce) return;
        alreadyAppliedForce = true;
        stateMachine
            .ForceReceiver
            .AddForce(stateMachine.transform.forward * 5f);
    }
}
