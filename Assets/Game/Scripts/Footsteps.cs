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

    public void FootR()
    {
        Debug.Log("FootR");
        if (stateMachine.InputReader.isRunning)
        {
            audioSource
                .PlayOneShot(footstepRunningClips[Random
                    .Range(0, footstepRunningClips.Length)]);
        }
        else
        {
            audioSource
                .PlayOneShot(footstepWalkingClips[Random
                    .Range(0, footstepWalkingClips.Length)]);
        }
    }

    public void FootL()
    {
        Debug.Log("FootL");
        if (stateMachine.InputReader.isRunning)
        {
            audioSource
                .PlayOneShot(footstepRunningClips[Random
                    .Range(0, footstepRunningClips.Length)]);
        }
        else
        {
            audioSource
                .PlayOneShot(footstepWalkingClips[Random
                    .Range(0, footstepWalkingClips.Length)]);
        }
    }
}
