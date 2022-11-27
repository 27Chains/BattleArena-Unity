using FishNet.Object;
using UnityEngine;

public class ForceReceiver : NetworkBehaviour
{
    private Vector3 impact;

    private Vector3 dampingVelocity;

    [SerializeField]
    private float drag = 0.3f;

    public Vector3 Movement => impact;

    private void Update()
    {
        impact =
            Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
    }

    [ServerRpc(RunLocally = true)]
    public void AddForce(Vector3 force)
    {
        impact += force;
    }
}
