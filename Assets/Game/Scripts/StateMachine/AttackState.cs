using System;
using UnityEngine;

public class AttackState : State
{
    private float timePassed;

    private int comboIndex = 0;

    private string currentAnimation;

    private bool alreadyAppliedForce;

    private WeaponSO weapon;

    public AttackState(StateMachine _stateMachine, Character _character) :
        base(_stateMachine, _character)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Enter Attack State");
        if (!character.IsOwner) return;
        weapon = character.Inventory.Weapon;
        currentAnimation = weapon.AttackAnimations[comboIndex];
        character.ServerPlayAnim (currentAnimation);
        character.InputReader.AttackEvent += TryComboAttack;
    }

    public override void LogicUpdate()
    {
        if (!character.IsOwner) return;
        base.LogicUpdate();
        timePassed = GetNormalizedTime(character.Animator, 0, currentAnimation);
        if (timePassed > 0.35f && !alreadyAppliedForce)
        {
            alreadyAppliedForce = true;
            TryApplyForce();
        }
        if (GetNormalizedTime(character.Animator, 0, currentAnimation) > 1f)
        {
            comboIndex = 0;
            stateMachine.ChangeState(PlayerState.Movement);
        }
    }

    public override void Exit()
    {
        timePassed = 0f;
        alreadyAppliedForce = false;
        character.InputReader.AttackEvent -= TryComboAttack;
    }

    private void TryComboAttack()
    {
        if (
            timePassed > weapon.ComboAttackTime[comboIndex] &&
            timePassed <
            weapon.ComboAttackTime[comboIndex] +
            weapon.ComboAttackWindow[comboIndex] &&
            weapon.AttackAnimations.Length > comboIndex + 1
        )
        {
            comboIndex++;
            stateMachine.ChangeState(PlayerState.Attack);
        }
    }

    private void TryApplyForce()
    {
        character
            .ForceReceiver
            .AddForce(character.transform.forward * weapon.GetWeaponForce(),
            ForceType.Smooth);
    }
}
