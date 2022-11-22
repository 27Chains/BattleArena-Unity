using Cinemachine;
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

    public Transform MainCameraTransform { get; private set; }

    public bool isOwner { get; private set; }

    public override void OnStartClient()
    {
        if (IsOwner)
        {
            isOwner = true;
            CinemachineVirtualCamera virtualCamera =
                FindObjectOfType<CinemachineVirtualCamera>();
            virtualCamera.Follow = transform;
        }

        SwitchState(new PlayerMovementState(this));
    }
}
