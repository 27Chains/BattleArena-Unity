using UnityEngine;

public struct MoveData
{
    public Vector3 Movement;
}

public struct ReconcileData
{
    public Vector3 Position;
}

public class PlayerMovementState : PlayerBaseState
{
    private readonly int
        MovementBlendTreeHash = Animator.StringToHash("MovementBlendTree");

    private readonly int
        MovementSpeedHash = Animator.StringToHash("MovementSpeed");

    public float smoothingValue = 0.1f;

    public PlayerMovementState(PlayerStateMachine stateMachine) :
        base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime (
            MovementBlendTreeHash,
            crossFadeDuration
        );
    }

    public override void Exit()
    {
    }

    public override void Tick(float deltaTime)
    {
        if (!stateMachine.IsOwner) return;

        if (stateMachine.Animator.GetFloat(MovementSpeedHash) < 0.01f)
        {
            stateMachine.Animator.SetFloat(MovementSpeedHash, 0f);
        }
        Vector3 movement = CalculateMovement();

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine
                .Animator
                .SetFloat(MovementSpeedHash, 0f, smoothingValue, deltaTime);
            return;
        }
        FaceMovementDirection (movement, deltaTime);
        stateMachine
            .Animator
            .SetFloat(MovementSpeedHash, 1f, smoothingValue, deltaTime);
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation =
            Quaternion
                .Lerp(stateMachine.transform.rotation,
                Quaternion.LookRotation(movement),
                deltaTime * stateMachine.RotationSpeed);
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();

        movement.x = stateMachine.InputReader.MovementValue.x;
        movement.y = 0;
        movement.z = stateMachine.InputReader.MovementValue.y;

        return movement;
    }
}
