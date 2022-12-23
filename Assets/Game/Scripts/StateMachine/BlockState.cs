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
    }

    public override void HandleInput()
    {
        if (!character.IsOwner) return;
        base.HandleInput();
        Vector3 movement = new Vector3();
        movement.x = character.InputReader.MovementValue.x;
        movement.y = 0;
        movement.z = character.InputReader.MovementValue.y;
        MoveData moveData = default;
        Vector3 mousePosition = GetMousePositionInWorld();
        Vector3 direction =
            (mousePosition - character.transform.position).normalized;
        Quaternion lookRotation =
            Quaternion
                .Slerp(character.transform.rotation,
                Quaternion.LookRotation(direction),
                0.35f);
        moveData.Rotation = lookRotation;

        if (movement != Vector3.zero)
        {
            moveData.Movement = movement.normalized * character.walkSpeed;
            character.MovementData = moveData;
        }
        else
        {
            character.MovementData = moveData;
        }
    }

    public override void LogicUpdate()
    {
        if (!character.IsOwner) return;
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
