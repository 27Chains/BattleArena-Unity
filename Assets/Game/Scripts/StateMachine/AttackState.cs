using System;
using UnityEngine;

public class AttackState : State
{
    private float timePassed;

    private int comboIndex = 0;

    private string currentAnimation;

    public AttackState(StateMachine _stateMachine, Character _character) :
        base(_stateMachine, _character)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        if (!character.IsOwner) return;
        currentAnimation =
            character.Inventory.Weapon.AttackAnimations[comboIndex];
        character.ServerPlayAnim (currentAnimation);
        character.InputReader.AttackEvent += TryComboAttack;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timePassed = GetNormalizedTime(character.Animator, 0, currentAnimation);
        if (GetNormalizedTime(character.Animator, 0, currentAnimation) > 1f)
        {
            comboIndex = 0;
            stateMachine.ChangeState(PlayerState.Movement);
        }
    }

    public override void Exit()
    {
        timePassed = 0f;
        character.InputReader.AttackEvent -= TryComboAttack;
    }

    private void TryComboAttack()
    {
        if (
            timePassed > 0.5f &&
            timePassed < 0.8f &&
            character.Inventory.Weapon.AttackAnimations.Length > comboIndex + 1
        )
        {
            comboIndex++;
            stateMachine.ChangeState(PlayerState.Attack);
        }
    }
}
