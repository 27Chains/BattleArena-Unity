using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        stateMachine.ServerCall();
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

    public override void LogicUpdate(
        MoveData moveData,
        bool asServer,
        bool replaying = false
    )
    {
        Move((float) stateMachine.TimeManager.TickDelta);
    }

    public override void Tick(float deltaTime)
    {
    }
}
