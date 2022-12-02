using Cinemachine;
using FishNet.Component.Animating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public enum PlayerState
{
    Movement,
    Attacking,
    ComboAttack1,
    ComboAttack2
}

public class PlayerStateMachine : NetworkBehaviour
{
    [field: SerializeField]
    public InputReader InputReader { get; private set; }

    [field: SerializeField]
    public CharacterController CharacterController { get; private set; }

    [field: SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField]
    public NetworkAnimator NetworkAnimator { get; private set; }

    [field: SerializeField]
    public Player Player { get; private set; }

    [field: SerializeField]
    public WeaponHandler WeaponHandler { get; private set; }

    [field: SerializeField]
    public Fighter Fighter { get; private set; }

    [field: SerializeField]
    public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField]
    public float RunningSpeed { get; private set; }

    [field: SerializeField]
    public float WalkingSpeed { get; private set; }

    [field: SerializeField]
    public float RotationSpeed { get; private set; }

    [SyncVar]
    [HideInInspector]
    private int _currentStateIndex;

    private State[] _states = new State[4];

    [HideInInspector]
    public PlayerState CurrentState => (PlayerState) _currentStateIndex;

    public MoveData MovementData;

    private void Awake()
    {
        _states[(int) PlayerState.Movement] = new PlayerMovementState(this);
        _states[(int) PlayerState.Attacking] =
            new PlayerAttackingState(this, 0);
        _states[(int) PlayerState.ComboAttack1] =
            new PlayerAttackingState(this, 1);
        _states[(int) PlayerState.ComboAttack2] =
            new PlayerAttackingState(this, 2);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsOwner) SwitchState(PlayerState.Movement);
    }

    private void Update()
    {
        _states[_currentStateIndex]?.Tick(Time.deltaTime);
    }

    [ServerRpc(RunLocally = true)]
    public void SwitchState(PlayerState state)
    {
        _states[_currentStateIndex]?.Exit();
        _currentStateIndex = (int) state;
        _states[_currentStateIndex]?.Enter();
    }

    public void MovementUpdate(
        MoveData moveData,
        bool asServer,
        bool replaying = false
    )
    {
        _states[_currentStateIndex]?
            .MovementUpdate(moveData, asServer, replaying);
    }
}
