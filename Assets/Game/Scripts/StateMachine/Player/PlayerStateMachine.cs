using Cinemachine;
using FishNet.Component.Animating;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public enum PlayerState
{
    Movement,
    Attacking,
    ComboAttack1,
    ComboAttack2,
    Dead,
    Impact
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
    public Health Health { get; private set; }

    [field: SerializeField]
    public Player Player { get; private set; }

    [field: SerializeField]
    public WeaponHandler WeaponHandler { get; private set; }

    [field: SerializeField]
    public Fighter Fighter { get; private set; }

    [field: SerializeField]
    public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField]
    public AudioSource AudioSource { get; private set; }

    [field: SerializeField]
    public float RunningSpeed { get; private set; }

    [field: SerializeField]
    public float WalkingSpeed { get; private set; }

    [field: SerializeField]
    public float RotationSpeed { get; private set; }

    [field: SerializeField]
    public float KnockbackDuration { get; private set; }

    [field: SerializeField]
    public AudioClip[] SwordWhooshClips { get; private set; }

    [field: SerializeField]
    public AudioClip[] SwordHitClips { get; private set; }

    [SyncVar]
    private int _currentStateIndex;

    private State[] _states = new State[6];

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
        _states[(int) PlayerState.Dead] = new PlayerDeadState(this);
        _states[(int) PlayerState.Impact] = new PlayerImpactState(this);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner) return;
        SwitchState(PlayerState.Movement);
        Health.OnDie += HandleDie;
        Health.OnTakeDamage += HandleTakeDamage;
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

    private void HandleDie()
    {
        SwitchState(PlayerState.Dead);
    }

    private void HandleTakeDamage(float damage)
    {
        if (Health.GetHealthPoints() <= 0) return;
        SwitchState(PlayerState.Impact);
    }
}
