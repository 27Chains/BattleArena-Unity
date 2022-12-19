using UnityEngine;

public class BlockState : State
{
    public BlockState(StateMachine _stateMachine, Character _character) :
        base(_stateMachine, _character)
    {
        stateMachine = _stateMachine;
        character = _character;
    }

    public override void Enter()
    {
        if (!character.IsOwner) return;
        character.ServerPlayAnim("Block");
    }

    public override void Exit()
    {
        character.ServerPlayAnim("BlockDefault");
    }

    public override void HandleInput()
    {
        if (!character.IsOwner) return;
        base.HandleInput();
        Vector3 movement = new Vector3();
        movement.x = character.InputReader.MovementValue.x;
        movement.y = 0;
        movement.z = character.InputReader.MovementValue.y;
        if (movement != Vector3.zero)
        {
            MoveData moveData = default;
            float MovementSpeed = character.walkSpeed;

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
        if (!character.IsOwner) return;
        if (!character.InputReader.IsBlocking)
        {
            stateMachine.ChangeState(PlayerState.Movement);
        }
    }
}
