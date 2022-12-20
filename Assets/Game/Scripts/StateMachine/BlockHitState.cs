using UnityEngine;

public class BlockHitState : State
{
    public BlockHitState(StateMachine _stateMachine, Character _character) :
        base(_stateMachine, _character)
    {
        stateMachine = _stateMachine;
        character = _character;
    }

    public override void Enter()
    {
        if (!character.IsOwner) return;
        character.ServerPlayAnim("BlockHit");
    }

    public override void Exit()
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!character.IsOwner) return;

        if (GetNormalizedTime(character.Animator, 0, "BlockHit") >= 0.9f)
        {
            if (character.InputReader.IsBlocking)
            {
                stateMachine.ChangeState(PlayerState.Block);
            }
            else
            {
                stateMachine.ChangeState(PlayerState.Movement);
            }
        }
    }
}
