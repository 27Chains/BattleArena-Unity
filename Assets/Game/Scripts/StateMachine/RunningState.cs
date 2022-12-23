using UnityEngine;

public class RunningState : State
{
    public RunningState(StateMachine _stateMachine, Character _character) :
        base(_stateMachine, _character)
    {
        stateMachine = _stateMachine;
        character = _character;
    }

    public override void Enter()
    {
        if (!character.IsOwner) return;
        character.ServerPlayAnim("Run");
    }

    public override void Exit()
    {
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
            moveData.Movement = movement.normalized * character.runningSpeed;

            moveData.Rotation =
                Quaternion
                    .Slerp(character.transform.rotation,
                    Quaternion.LookRotation(movement),
                    0.35f);

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
        if (!character.IsOwner) return;
        if (!character.InputReader.IsRunning)
        {
            stateMachine.ChangeState(PlayerState.Movement);
        }
    }
}
