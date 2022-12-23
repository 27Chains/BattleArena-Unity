using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackState : State
{
    private float timePassed;

    private int comboIndex = 0;

    private string currentAnimation;

    private WeaponSO weapon;

    public AttackState(StateMachine _stateMachine, Character _character) :
        base(_stateMachine, _character)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        if (!character.IsOwner) return;
        weapon = character.Inventory.Weapon;
        currentAnimation = weapon.AttackAnimations[comboIndex];
        character.ServerPlayAnim (currentAnimation);
        character.InputReader.AttackEvent += TryComboAttack;
    }

    public override void LogicUpdate()
    {
        if (!character.IsOwner) return;
        Vector3 mousePosition = GetMousePositionInWorld();
        Vector3 direction =
            (mousePosition - stateMachine.transform.position).normalized;
        Quaternion lookRotation =
            Quaternion
                .Slerp(stateMachine.transform.rotation,
                Quaternion.LookRotation(direction),
                0.15f);

        character.MovementData.Rotation = lookRotation;
        timePassed = GetNormalizedTime(character.Animator, 2, currentAnimation);
        if (timePassed > 1f)
        {
            comboIndex = 0;
            stateMachine.ChangeState(PlayerState.Movement);
        }
    }

    public override void Exit()
    {
        if (!character.IsOwner) return;
        timePassed = 0f;
        character.ServerPlayAnim("DefaultAttackState");
        character.InputReader.AttackEvent -= TryComboAttack;
    }

    public override void HandleInput()
    {
        base.HandleInput();
        if (!character.IsOwner) return;
        Vector3 movement = new Vector3();
        movement.x = character.InputReader.MovementValue.x;
        movement.y = 0;
        movement.z = character.InputReader.MovementValue.y;
        if (movement != Vector3.zero)
        {
            MoveData moveData = default;
            moveData.Movement = movement.normalized * character.walkSpeed;
            character.MovementData = moveData;
        }
        else
        {
            character.MovementData.Movement = Vector3.zero;
        }
    }

    private void TryComboAttack()
    {
        Debug.Log("TryComboAttack");
        if (
            weapon.AttackAnimations.Length > comboIndex + 1 &&
            timePassed > weapon.ComboAttackTime[comboIndex] &&
            timePassed <
            weapon.ComboAttackTime[comboIndex] +
            weapon.ComboAttackWindow[comboIndex]
        )
        {
            comboIndex++;
            stateMachine.ChangeState(PlayerState.Attack);
        }
    }
}
