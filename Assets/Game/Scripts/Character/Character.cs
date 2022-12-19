using System;
using Cinemachine;
using FishNet.Object;
using FishNet.Object.Prediction;
using UnityEngine;

public struct MoveData
{
    public Vector3 Movement;

    public bool IsRunning;
}

public struct ReconcileData
{
    public Vector3 Position;

    public Quaternion Rotation;
}

public enum PlayerState
{
    Movement,
    Attack,
    Dodge,
    Block,
    Death
}

public class Character : NetworkBehaviour
{
    [Header("Controls")]
    public float walkSpeed = 5f;

    public float turnSpeed = 5f;

    public float dodgeSpeed = 10f;

    public float runningSpeed = 10f;

    [Header("Animation Smoothing")]
    [Range(0, 1)]
    public float speedDampTime = 0.1f;

    [Range(0, 1)]
    public float velocityDampTime = 0.9f;

    [Range(0, 1)]
    public float rotationDampTime = 0.2f;

    [HideInInspector]
    public Animator Animator;

    [HideInInspector]
    public CharacterController CharacterController;

    [HideInInspector]
    public CharacterInventory Inventory;

    [HideInInspector]
    public InputReader InputReader;

    [HideInInspector]
    public ForceReceiver ForceReceiver;

    [HideInInspector]
    public Health Health;

    [HideInInspector]
    public StateMachine stateMachine;

    public MovementState movementState;

    public MoveData MovementData;

    public State[] states = new State[5];

    public State GetState(PlayerState state)
    {
        return states[(int) state];
    }

    private void Awake()
    {
        InputReader = GetComponent<InputReader>();
        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
        Inventory = GetComponent<CharacterInventory>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Health = GetComponent<Health>();
        stateMachine = GetComponent<StateMachine>();

        states[(int) PlayerState.Movement] =
            new MovementState(stateMachine, this);
        states[(int) PlayerState.Attack] = new AttackState(stateMachine, this);
        states[(int) PlayerState.Dodge] = new DodgeState(stateMachine, this);
        states[(int) PlayerState.Block] = new BlockState(stateMachine, this);
        states[(int) PlayerState.Death] = new DeadState(stateMachine, this);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner) return;
        CinemachineVirtualCamera virtualCamera =
            FindObjectOfType<CinemachineVirtualCamera>();
        virtualCamera.Follow = transform.GetChild(0).transform;
        stateMachine.Initialize(states[(int) PlayerState.Movement]);
        InitializeHUD();
        Health.OnDie += HandleDeath;
    }

    private void HandleDeath()
    {
        stateMachine.ChangeState(PlayerState.Death);
    }

    private void InitializeHUD()
    {
        if (!IsOwner) return;
        PlayerHUD hud = FindObjectOfType<PlayerHUD>();
        hud.Initialize (Health);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        stateMachine.Initialize(states[(int) PlayerState.Movement]);
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

    [ServerRpc(RunLocally = true)]
    public void ServerPlayAnim(string animName)
    {
        int animHash = Animator.StringToHash(animName);
        Animator.CrossFadeInFixedTime(animHash, 0.1f);
        if (IsServer)
        {
            ObserversPlayAnim (animHash);
        }
    }

    [ObserversRpc(IncludeOwner = false)]
    public void ObserversPlayAnim(int animName)
    {
        Animator.CrossFadeInFixedTime(animName, 0.1f);
    }

    [Replicate]
    private void Move(MoveData moveData, bool asServer, bool replaying = false)
    {
        float deltaTime = (float) base.TimeManager.TickDelta;

        if (moveData.Movement != Vector3.zero)
        {
            float movementSpeed = moveData.IsRunning ? 1f : 0.5f;
            Animator
                .SetFloat("MovementSpeed",
                movementSpeed,
                speedDampTime,
                deltaTime);

            transform.rotation = Quaternion.LookRotation(moveData.Movement);
        }
        else
        {
            Animator.SetFloat("MovementSpeed", 0f, speedDampTime, deltaTime);
        }

        if (Animator.GetFloat("MovementSpeed") < 0.01f)
        {
            Animator.SetFloat("MovementSpeed", 0f);
        }

        CharacterController.Move(moveData.Movement * deltaTime);
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

    private void Update()
    {
        if (!IsOwner) return;
        stateMachine.currentState.HandleInput();
        stateMachine.currentState.LogicUpdate();
    }
}
