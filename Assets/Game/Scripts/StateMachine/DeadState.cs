public class DeadState : State
{
    public DeadState(StateMachine _stateMachine, Character _character) :
        base(_stateMachine, _character)
    {
        stateMachine = _stateMachine;
        character = _character;
    }

    public override void Enter()
    {
        if (!stateMachine.IsOwner) return;
        character.ServerPlayAnim("Death");
        character.CharacterController.enabled = false;
        character.CharacterController.detectCollisions = false;
    }

    public override void Exit()
    {
    }
}
