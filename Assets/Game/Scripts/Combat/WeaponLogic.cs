using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    [SerializeField]
    private Collider myCollider;

    [SerializeField]
    private Fighter fighter;

    private List<Collider> alreadyCollidedWith = new List<Collider>();

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) return;
        if (alreadyCollidedWith.Contains(other)) return;
        alreadyCollidedWith.Add (other);
        if (other.TryGetComponent<Health>(out Health health))
        {
            print("Hit " + other.name);
            health.TakeDamage(fighter.CurrentWeapon.GetDamage());
        }
        if (
            other
                .TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver)
        )
        {
            Vector3 direction =
                (other.transform.position - myCollider.transform.position)
                    .normalized;

            forceReceiver
                .AddForce(direction *
                fighter.CurrentWeapon.GetKnockbackForce());
        }
    }
}
