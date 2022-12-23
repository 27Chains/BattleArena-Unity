using System;
using UnityEngine;

public class DodgeState : State
{
    private bool alreadyAppliedForce;

    public DodgeState(StateMachine _stateMachine, Character _character) :
        base(_stateMachine, _character)
    {
        stateMachine = _stateMachine;
        character = _character;
    }

    public override void Enter()
    {
        if (!character.IsOwner) return;
        character.ServerPlayAnim("Dodge");
        Dodge();
    }

    private void Dodge()
    {
        Vector3 movement = new Vector3();
        movement.x = character.InputReader.MovementValue.x;
        movement.y = 0;
        movement.z = character.InputReader.MovementValue.y;
        character.MovementData.Rotation = Quaternion.LookRotation(movement);

        character
            .ForceReceiver
            .AddForce(movement * character.dodgeSpeed, ForceType.Linear, 0.65f);
    }

    public override void Exit()
    {
        alreadyAppliedForce = false;
    }

    public override void LogicUpdate()
    {
        if (!character.IsOwner) return;
        if (GetNormalizedTime(character.Animator, 0, "Dodge") > 0.75f)
        {
            stateMachine.ChangeState(PlayerState.Movement);
        }
    }
}
