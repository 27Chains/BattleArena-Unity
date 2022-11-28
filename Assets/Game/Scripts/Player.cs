using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FishNet.Object;
using FishNet.Object.Prediction;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private PlayerStateMachine _stateMachine;

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

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner) return;
        CinemachineVirtualCamera virtualCamera =
            FindObjectOfType<CinemachineVirtualCamera>();
        virtualCamera.Follow = transform.GetChild(0).transform;
    }

    [Replicate]
    private void Move(MoveData moveData, bool asServer, bool replaying = false)
    {
        _stateMachine.MovementUpdate (moveData, asServer, replaying);
    }

    [ServerRpc(RunLocally = true)]
    public void ServerPlayAnim(int animName, float transitionDuration)
    {
        _stateMachine.Animator.CrossFadeInFixedTime (
            animName,
            transitionDuration
        );
        ObserversPlayAnim (animName, transitionDuration);
    }

    [ObserversRpc(IncludeOwner = false)]
    public void ObserversPlayAnim(int animName, float transitionDuration)
    {
        _stateMachine.Animator.CrossFadeInFixedTime (
            animName,
            transitionDuration
        );
    }

    [Reconcile]
    private void Reconcile(ReconcileData recData, bool asServer)
    {
        transform.position = recData.Position;
        transform.rotation = recData.Rotation;
    }

    private void TimeManager_OnTick()
    {
        if (IsOwner)
        {
            Reconcile(default, false);
            Move(_stateMachine.MovementData, false);
        }
        if (IsServer)
        {
            Move(default, true);
            ReconcileData rd =
                new ReconcileData()
                {
                    Position = transform.position,
                    Rotation = transform.rotation
                };
            Reconcile(rd, true);
        }
    }
}
