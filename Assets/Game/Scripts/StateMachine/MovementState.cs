using System;
using UnityEngine;

public class MovementState : State
{
    private string moveAnimation = "MovementBlendTree";

    private bool leavingState;

    public MovementState(StateMachine _stateMachine, Character _character) :
        base(_stateMachine, _character)
    {
        stateMachine = _stateMachine;
        character = _character;
    }

    public override void Enter()
    {
        if (!character.IsOwner) return;
        character.ServerPlayAnim (moveAnimation);
        character.InputReader.AttackEvent += OnAttack;
        character.InputReader.DodgeEvent += OnDodge;
    }

    private void OnDodge()
    {
        if (character.MovementData.Movement == Vector3.zero) return;
        stateMachine.ChangeState(PlayerState.Dodge);
    }

    private void OnAttack()
    {
        stateMachine.ChangeState(PlayerState.Attack);
    }

    public override void Exit()
    {
        character.InputReader.AttackEvent -= OnAttack;
        character.InputReader.DodgeEvent -= OnDodge;
    }

    public override void HandleInput()
    {
        base.HandleInput();
        if (!character.IsOwner) return;
        Vector3 movement = new Vector3();
        movement.x = character.InputReader.MovementValue.x;
        movement.y = 0;
        movement.z = character.InputReader.MovementValue.y;
        MoveData moveData = default;
        Vector3 mousePosition = GetMousePositionInWorld();
        Vector3 direction =
            (mousePosition - character.transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation =
                Quaternion
                    .Slerp(character.transform.rotation,
                    Quaternion.LookRotation(direction),
                    0.35f);
            moveData.Rotation = lookRotation;
        }

        if (movement != Vector3.zero)
        {
            moveData.Movement = movement.normalized * character.walkSpeed;
            character.MovementData = moveData;
        }
        else
        {
            character.MovementData = moveData;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!character.IsOwner) return;

        if (character.InputReader.IsBlocking)
        {
            stateMachine.ChangeState(PlayerState.Block);
        }
        if (
            character.MovementData.Movement != Vector3.zero &&
            character.InputReader.IsRunning
        )
        {
            stateMachine.ChangeState(PlayerState.Running);
        }
    }
}
