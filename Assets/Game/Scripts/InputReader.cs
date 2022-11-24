using System;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : NetworkBehaviour, PlayerControls.IPlayerActions
{
    public Vector2 MovementValue { get; private set; }

    private PlayerControls playerControls;

    public event Action AttackEvent;

    public bool isRunning { get; private set; }

    private void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Player.SetCallbacks(this);

        playerControls.Player.Enable();
    }

    private void OnDestroy()
    {
        playerControls.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRunning = true;
        }
        else if (context.canceled)
        {
            isRunning = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && IsOwner)
        {
            ServerAttackEventHandler();
        }
    }

    [ServerRpc(RunLocally = true)]
    private void ServerAttackEventHandler()
    {
        if (IsServer)
        {
            ObserversAttack();
        }

        AttackEvent?.Invoke();
    }

    [ObserversRpc(IncludeOwner = false)]
    private void ObserversAttack()
    {
        AttackEvent?.Invoke();
    }
}
