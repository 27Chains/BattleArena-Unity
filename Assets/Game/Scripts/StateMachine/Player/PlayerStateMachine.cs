using Cinemachine;
using FishNet.Component.Animating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public enum PlayerState
{
    Movement,
    Attacking
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
    public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField]
    public float RunningSpeed { get; private set; }

    [field: SerializeField]
    public float WalkingSpeed { get; private set; }

    [field: SerializeField]
    public float RotationSpeed { get; private set; }

    private State _currentState;

    private State[] _states = new State[2];

    public State CurrentState => _currentState;

    public MoveData MovementData;

    private void Awake()
    {
        _states[(int) PlayerState.Movement] = new PlayerMovementState(this);
        _states[(int) PlayerState.Attacking] = new PlayerAttackingState(this);
    }

    private void Start()
    {
        SwitchState(PlayerState.Movement);
    }

    [SyncVar]
    private int _currentStateIndex;

    private void Update()
    {
        _currentState?.Tick(Time.deltaTime);
    }

    [ServerRpc(RunLocally = true)]
    public void SwitchState(PlayerState state)
    {
        _currentState?.Exit();
        _currentState = _states[(int) state];
        _currentState?.Enter();
    }

    public void MovementUpdate(
        MoveData moveData,
        bool asServer,
        bool replaying = false
    )
    {
        _currentState?.MovementUpdate(moveData, asServer, replaying);
    }

    [ServerRpc(RunLocally = true)]
    public void CrossFadeAnimation(
        string animationName,
        float crossFadeDuration
    )
    {
        Animator.CrossFadeInFixedTime (animationName, crossFadeDuration);
    }
}
