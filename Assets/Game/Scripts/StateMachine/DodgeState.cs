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
        Debug.Log("Enter Dodge State");
        if (!character.IsOwner) return;
        character.ServerPlayAnim("Dodge");
    }

    private void Dodge()
    {
        character
            .ForceReceiver
            .AddForce(character.transform.forward * character.dodgeSpeed,
            ForceType.Linear,
            0.65f);
    }

    public override void Exit()
    {
        alreadyAppliedForce = false;
    }

    public override void LogicUpdate()
    {
        if (!alreadyAppliedForce)
        {
            Dodge();
            alreadyAppliedForce = true;
        }
        if (GetNormalizedTime(character.Animator, 0, "Dodge") > 0.75f)
        {
            stateMachine.ChangeState(PlayerState.Movement);
        }
    }
}
