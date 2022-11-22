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
        Vector3 movement = CalculateMovement();

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(MovementSpeedHash, 0f);
            return;
        }
        FaceMovementDirection (movement, deltaTime);
        stateMachine
            .Animator
            .SetFloat(MovementSpeedHash, 1f, smoothingValue, deltaTime);
    }

    public override void TimeManagerTick()
    {
        if (stateMachine.IsOwner)
        {
            Reconcile(default, false);
            BuildActions(out MoveData md);
            Move(md, false);
        }
        if (stateMachine.IsServer)
        {
            Debug.Log("Tick on the server");
            Move(default, true);
            ReconcileData rd =
                new ReconcileData()
                { Position = stateMachine.transform.position };
            Reconcile(rd, true);
        }
    }

    private void BuildActions(out MoveData moveData)
    {
        moveData = default;
        moveData.Movement = CalculateMovement() * stateMachine.RunningSpeed;
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
