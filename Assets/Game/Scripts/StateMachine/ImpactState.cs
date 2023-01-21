using UnityEngine;

public class ImpactState : State
{
    private bool alreadyAppliedForce;

    public ImpactState(StateMachine _stateMachine, Character _character) :
        base(_stateMachine, _character)
    {
        stateMachine = _stateMachine;
        character = _character;
    }

    public override void Enter()
    {
        if (!character.IsOwner) return;
        alreadyAppliedForce = false;
        character.ServerPlayAnim("Impact");
    }

    public override void Exit()
    {
    }

    public void AddImpactForce()
    {
        Vector3 direction = character.transform.position - character.HitData;
        character
            .ForceReceiver
            .AddForce(direction *
            character.Inventory.Weapon.GetKnockbackForce(),
            ForceType.Smooth);
    }

    public override void LogicUpdate()
    {
        if (!alreadyAppliedForce)
        {
            AddImpactForce();
            alreadyAppliedForce = true;
        }
        base.LogicUpdate();
        if (!character.IsOwner) return;
        if (GetNormalizedTime(character.Animator, 0, "Impact") >= 0.9f)
        {
            stateMachine.ChangeState(PlayerState.Movement);
        }
    }
}
