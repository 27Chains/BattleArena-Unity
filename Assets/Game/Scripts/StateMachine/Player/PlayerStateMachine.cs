using Cinemachine;
using FishNet.Object.Prediction;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField]
    public InputReader InputReader { get; private set; }

    [field: SerializeField]
    public CharacterController CharacterController { get; private set; }

    [field: SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField]
    public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField]
    public float RunningSpeed { get; private set; }

    [field: SerializeField]
    public float WalkingSpeed { get; private set; }

    [field: SerializeField]
    public float RotationSpeed { get; private set; }

    public MoveData MovementData;

    public Transform MainCameraTransform { get; private set; }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsOwner)
        {
            CinemachineVirtualCamera virtualCamera =
                FindObjectOfType<CinemachineVirtualCamera>();
            virtualCamera.Follow = transform.GetChild(0).transform;
        }
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

    [Replicate]
    private void Move(MoveData moveData, bool asServer, bool replaying = false)
    {
        MovementUpdate (moveData, asServer, replaying);
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
            Move(MovementData, false);
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

    private void Start()
    {
        SwitchState(new PlayerMovementState(this));
    }
}
