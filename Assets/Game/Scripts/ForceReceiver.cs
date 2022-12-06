using FishNet.Object;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    private Vector3 impact;

    private Vector3 dampingVelocity;

    [SerializeField]
    private float drag = 0.1f;

    public Vector3 Movement => impact;

    private void Update()
    {
        impact =
            Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (impact.magnitude < 0.1f)
        {
            drag = 0.1f;
        }
    }

    public void AddForce(Vector3 force, float drag = 0.1f)
    {
        this.drag = drag;
        impact += force;
    }
}
