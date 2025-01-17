using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerControls.IPlayerActions
{
    public Vector2 MovementValue { get; private set; }

    private PlayerControls playerControls;

    public event Action AttackEvent;

    public event Action ShowWeaponEvent;

    public event Action DodgeEvent;

    public event Action<Vector2> DashEvent;

    public bool IsRunning { get; private set; }

    public bool IsBlocking { get; private set; }

    private float lastTapTime;

    private const float doubleTapTime = 0.2f;

    private void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Player.SetCallbacks(this);

        playerControls.Player.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public Vector3 GetDirection()
    {
        return new Vector3(MovementValue.x, 0, MovementValue.y);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AttackEvent?.Invoke();
        }
    }

    public void OnShowWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShowWeaponEvent?.Invoke();
        }
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DodgeEvent?.Invoke();
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsBlocking = true;
        }
        else if (context.canceled)
        {
            IsBlocking = false;
        }
    }

    public void OnDoubleTap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float timeSinceLastTap = Time.time - lastTapTime;

            if (timeSinceLastTap <= doubleTapTime)
            {
                if (Keyboard.current.sKey.isPressed)
                {
                    Debug.Log("Dash forward");
                    // DashEvent?.Invoke(Vector3.back);
                }
            }

            lastTapTime = Time.time;
        }
    }
}
