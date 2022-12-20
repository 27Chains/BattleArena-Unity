using System;
using Cinemachine;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Prediction;
using UnityEngine;

public struct MoveData
{
    public Vector3 Movement;

    public Quaternion Rotation;

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
    Death,
    Impact,
    BlockHit
}

public class Character : NetworkBehaviour
{
    [Header("Controls")]
    public float walkSpeed = 5f;

    public float blockingMoveSpeed = 2f;

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
    public BlockingCollider BlockingCollider;

    [HideInInspector]
    public Health Health;

    [HideInInspector]
    public StateMachine stateMachine;

    public MovementState movementState;

    public MoveData MovementData;

    public Vector3 HitData;

    public State[] states = new State[7];

    public int MovementSpeedAnimHash = Animator.StringToHash("MovementSpeed");

    public int
        VerticalMovementAnimHash = Animator.StringToHash("VerticalMovement");

    public int
        HorizontalMovementAnimHash =
            Animator.StringToHash("HorizontalMovement");

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
        BlockingCollider = GetComponentInChildren<BlockingCollider>();
        Health = GetComponent<Health>();
        stateMachine = GetComponent<StateMachine>();

        states[(int) PlayerState.Movement] =
            new MovementState(stateMachine, this);
        states[(int) PlayerState.Attack] = new AttackState(stateMachine, this);
        states[(int) PlayerState.Dodge] = new DodgeState(stateMachine, this);
        states[(int) PlayerState.Block] = new BlockState(stateMachine, this);
        states[(int) PlayerState.Death] = new DeadState(stateMachine, this);
        states[(int) PlayerState.Impact] = new ImpactState(stateMachine, this);
        states[(int) PlayerState.BlockHit] =
            new BlockHitState(stateMachine, this);
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
        Health.OnTakeDamage += HandleDamage;
    }

    private void HandleDamage(float damage, Vector3 attackerPosition)
    {
        HitData = attackerPosition;
        if (InputReader.IsBlocking)
        {
            stateMachine.ChangeState(PlayerState.BlockHit);
        }
        else
        {
            stateMachine.ChangeState(PlayerState.Impact);
        }
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
    public void ServerRotate(Quaternion rotation)
    {
        transform.rotation = rotation;
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

    [ServerRpc(RunLocally = true)]
    public void ServerSetBool(string boolName, bool value)
    {
        Animator.SetBool (boolName, value);
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
        if (stateMachine.currentState == states[(int) PlayerState.Block])
        {
            float horizontalMovement =
                Vector3.Dot(transform.right, moveData.Movement);
            float verticalMovement =
                Vector3.Dot(transform.forward, moveData.Movement);
            Animator
                .SetFloat(HorizontalMovementAnimHash,
                horizontalMovement,
                0.1f,
                deltaTime);
            Animator
                .SetFloat(VerticalMovementAnimHash,
                verticalMovement,
                0.1f,
                deltaTime);

            CharacterController
                .Move((moveData.Movement * blockingMoveSpeed) * deltaTime);
            return;
        }

        if (moveData.Movement != Vector3.zero)
        {
            float movementSpeed = moveData.IsRunning ? 1f : 0.5f;
            Animator.SetFloat (
                MovementSpeedAnimHash,
                movementSpeed,
                speedDampTime,
                deltaTime
            );

            if (stateMachine.currentState == states[(int) PlayerState.Impact])
            {
                transform.rotation =
                    Quaternion.LookRotation(-moveData.Movement);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(moveData.Movement);
            }
        }
        else
        {
            Animator
                .SetFloat(MovementSpeedAnimHash, 0f, speedDampTime, deltaTime);
        }

        if (Animator.GetFloat(MovementSpeedAnimHash) < 0.01f)
        {
            Animator.SetFloat(MovementSpeedAnimHash, 0f);
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
