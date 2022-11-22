using FishNet.Object;
using FishNet.Object.Prediction;
using UnityEngine;

public class NetworkMovement : NetworkBehaviour
{
    [SerializeField]
    PlayerStateMachine stateMachine;

    [Replicate]
    private void Move(MoveData moveData, bool asServer, bool replaying = false)
    {
        float deltaTime = (float) stateMachine.TimeManager.TickDelta;
        Vector3 movement = (moveData.Movement);

        stateMachine.CharacterController.Move(movement * deltaTime);
    }

    [Reconcile]
    private void Reconcile(ReconcileData recData, bool asServer)
    {
        stateMachine.transform.position = recData.Position;
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        base.TimeManager.OnTick += TimeManager_OnTick;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        if (base.TimeManager != null)
            base.TimeManager.OnTick -= TimeManager_OnTick;
    }

    private void BuildActions(out MoveData moveData)
    {
        moveData = default;
        moveData.Movement = CalculateMovement() * stateMachine.RunningSpeed;
    }

    private void TimeManager_OnTick()
    {
        if (stateMachine.IsOwner)
        {
            Reconcile(default, false);
            BuildActions(out MoveData md);
            Move(md, false);
        }
        if (stateMachine.IsServer)
        {
            Move(default, true);
            ReconcileData rd =
                new ReconcileData()
                { Position = stateMachine.transform.position };
            Reconcile(rd, true);
        }
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();

        movement.x = stateMachine.InputReader.MovementValue.x;
        movement.y = 0;
        movement.z = stateMachine.InputReader.MovementValue.y;

        return movement;
    }
}
