using UnityEngine;

public struct MoveData
{
    public Vector3 Movement;

    public bool IsRunning;
}

public struct ReconcileData
{
    public Vector3 Position;

    public Quaternion Rotation;
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

        BuildActions(out MoveData md);
        stateMachine.MovementData = md;
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation =
            Quaternion
                .Lerp(stateMachine.transform.rotation,
                Quaternion.LookRotation(movement),
                deltaTime * stateMachine.RotationSpeed);
    }

    private void BuildActions(out MoveData moveData)
    {
        moveData = default;
        float MovementSpeed =
            stateMachine.InputReader.isRunning
                ? stateMachine.RunningSpeed
                : stateMachine.WalkingSpeed;
        moveData.Movement = CalculateMovement() * MovementSpeed;
        moveData.IsRunning = stateMachine.InputReader.isRunning;
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();

        movement.x = stateMachine.InputReader.MovementValue.x;
        movement.y = 0;
        movement.z = stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    public override void LogicUpdate(
        MoveData moveData,
        bool asServer,
        bool replaying = false
    )
    {
        float deltaTime = (float) stateMachine.TimeManager.TickDelta;
        if (moveData.Movement != Vector3.zero)
        {
            float MovementSpeed = moveData.IsRunning ? 1f : 0.5f;

            stateMachine.Animator.SetFloat (
                MovementSpeedHash,
                MovementSpeed,
                smoothingValue,
                deltaTime
            );
            FaceMovementDirection(moveData.Movement, deltaTime);
        }
        else
        {
            stateMachine
                .Animator
                .SetFloat(MovementSpeedHash, 0f, smoothingValue, deltaTime);
        }
        if (stateMachine.Animator.GetFloat(MovementSpeedHash) < 0.01f)
        {
            stateMachine.Animator.SetFloat(MovementSpeedHash, 0f);
        }

        stateMachine.CharacterController.Move(moveData.Movement * deltaTime);
    }
}
