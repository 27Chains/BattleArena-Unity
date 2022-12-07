using UnityEngine;

public enum ForceType
{
    Linear,
    Smooth
}

public class ForceReceiver : MonoBehaviour
{
    private Vector3 impact;

    private Vector3 dampingVelocity;

    private ForceType currentForceType = ForceType.Smooth;

    private float duration;

    [SerializeField]
    private float drag = 0.1f;

    public Vector3 Movement => impact;

    private void Update()
    {
        ReduceImpact();
    }

    private void ReduceImpact()
    {
        if (currentForceType == ForceType.Linear && duration > 0)
        {
            impact =
                Vector3.Lerp(impact, Vector3.zero, Time.deltaTime / duration);
            duration -= Time.deltaTime;
        }
        else if (currentForceType == ForceType.Smooth)
        {
            impact =
                Vector3
                    .SmoothDamp(impact,
                    Vector3.zero,
                    ref dampingVelocity,
                    drag);
        }
        if (impact.magnitude < 0.01f)
        {
            impact = Vector3.zero;
        }
    }

    public void AddForce(Vector3 force, ForceType forceType, float duration = 0)
    {
        if (forceType != currentForceType)
        {
            currentForceType = forceType;
        }
        this.duration = duration;
        impact += force;
    }
}
