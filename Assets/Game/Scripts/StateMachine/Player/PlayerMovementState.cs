using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (!stateMachine.isOwner) return;
        Vector3 movement = CalculateMovement();
        float movementSpeed =
            stateMachine.InputReader.isRunning
                ? stateMachine.RunningSpeed
                : stateMachine.WalkingSpeed;
        Move(movement * movementSpeed, deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(MovementSpeedHash, 0f);
            return;
        }
        stateMachine
            .Animator
            .SetFloat(MovementSpeedHash,
            stateMachine.InputReader.isRunning ? 1f : 0.5f,
            smoothingValue,
            deltaTime);

        FaceMovementDirection (movement, deltaTime);
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
