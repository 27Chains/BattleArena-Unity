using UnityEngine;

public class MovementState : State
{
    private string moveAnimation = "MovementBlendTree";

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
    }

    private void OnAttack()
    {
        stateMachine.ChangeState(PlayerState.Attack);
    }

    public override void Exit()
    {
        character.MovementData.Movement = Vector3.zero;
        character.InputReader.AttackEvent -= OnAttack;
    }

    public override void HandleInput()
    {
        base.HandleInput();
        Vector3 movement = new Vector3();
        movement.x = character.InputReader.MovementValue.x;
        movement.y = 0;
        movement.z = character.InputReader.MovementValue.y;
        if (movement != Vector3.zero)
        {
            MoveData moveData = default;
            float MovementSpeed =
                character.InputReader.IsRunning
                    ? character.runningSpeed
                    : character.walkSpeed;

            moveData.Movement = movement.normalized * MovementSpeed;
            moveData.IsRunning = character.InputReader.IsRunning;

            character.MovementData = moveData;
        }
        else
        {
            character.MovementData.Movement = Vector3.zero;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
