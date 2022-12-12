using FishNet.Object;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField]
    private PlayerStateMachine stateMachine;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] footstepWalkingClips;

    [SerializeField]
    private AudioClip[] footstepRunningClips;

    private int lastStep = 0;

    public void FootR()
    {
        if (!stateMachine.IsOwner) return;
        if (lastStep == 0) return;
        lastStep = 0;
        audioSource
            .PlayOneShot(footstepWalkingClips[Random
                .Range(0, footstepWalkingClips.Length)]);
    }

    public void FootL()
    {
        if (!stateMachine.IsOwner) return;
        if (lastStep == 1) return;
        lastStep = 1;
        audioSource
            .PlayOneShot(footstepWalkingClips[Random
                .Range(0, footstepWalkingClips.Length)]);
    }
}
