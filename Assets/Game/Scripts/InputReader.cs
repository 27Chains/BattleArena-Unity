using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : NetworkBehaviour, PlayerControls.IPlayerActions
{
    public Vector2 MovementValue { get; private set; }

    private PlayerControls playerControls;

    public event Action AttackEvent;

    [field: SyncVar]
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
        if (!IsOwner) return;
        if (context.performed)
        {
            SetIsRunning(true);
        }
        else if (context.canceled)
        {
            SetIsRunning(false);
        }
    }

    [ServerRpc]
    private void SetIsRunning(bool isRunning)
    {
        this.isRunning = isRunning;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AttackEvent?.Invoke();
        }
    }
}
