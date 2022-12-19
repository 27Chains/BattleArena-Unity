public class ImpactState : State
{
    public ImpactState(StateMachine _stateMachine, Character _character) :
        base(_stateMachine, _character)
    {
        stateMachine = _stateMachine;
        character = _character;
    }

    public override void Enter()
    {
        if (!character.IsOwner) return;
        character.ServerPlayAnim("Impact");
    }

    public override void Exit()
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!character.IsOwner) return;

        if (GetNormalizedTime(character.Animator, 0, "Impact") >= 0.9f)
        {
            stateMachine.ChangeState(PlayerState.Movement);
        }
    }
}
