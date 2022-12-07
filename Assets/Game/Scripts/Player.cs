using System;
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
        InitializeHUD();
        CinemachineVirtualCamera virtualCamera =
            FindObjectOfType<CinemachineVirtualCamera>();
        virtualCamera.Follow = transform.GetChild(0).transform;
    }

    private void InitializeHUD()
    {
        if (IsOwner)
        {
            PlayerHUD hud = FindObjectOfType<PlayerHUD>();
            hud.Initialize(this);
        }
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
        if (IsServer)
        {
            ObserversPlayAnim (animName, transitionDuration);
        }
    }

    [ObserversRpc(IncludeOwner = false)]
    public void ObserversPlayAnim(int animName, float transitionDuration)
    {
        _stateMachine.Animator.CrossFadeInFixedTime (
            animName,
            transitionDuration
        );
    }

    [ServerRpc(RunLocally = true)]
    public void ServerRotatePlayer(Vector3 direction)
    {
        _stateMachine.transform.rotation = Quaternion.LookRotation(direction);
    }

    [ServerRpc(RunLocally = true)]
    public void ServerApplyForce(
        Vector3 direction,
        float force,
        ForceType forceType,
        float duration = 0f
    )
    {
        _stateMachine
            .ForceReceiver
            .AddForce(direction * force, forceType, duration);
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
