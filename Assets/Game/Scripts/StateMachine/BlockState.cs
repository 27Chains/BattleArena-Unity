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
        if (character.IsServer)
        {
            character.BlockingCollider.EnableCollider();
        }
        if (!character.IsOwner) return;
        character.ServerSetBool("Blocking", true);
        character.ServerPlayAnim("Block");
    }

    public override void Exit()
    {
        if (character.IsServer)
        {
            character.BlockingCollider.DisableCollider();
        }
        if (!character.IsOwner) return;

        character.ServerPlayAnim("BlockDefault");
        character.ServerSetBool("Blocking", false);
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
            moveData.Movement = movement.normalized;
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
