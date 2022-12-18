using FishNet.Object;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] footstepWalkingClips;

    private int lastStep = 0;

    public void FootR()
    {
        if (lastStep == 0) return;
        lastStep = 0;
        audioSource
            .PlayOneShot(footstepWalkingClips[Random
                .Range(0, footstepWalkingClips.Length)]);
    }

    public void FootL()
    {
        if (lastStep == 1) return;
        lastStep = 1;
        audioSource
            .PlayOneShot(footstepWalkingClips[Random
                .Range(0, footstepWalkingClips.Length)]);
    }
}
