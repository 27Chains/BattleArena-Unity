using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class WeaponLogic : NetworkBehaviour
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
            health.TakeDamage(fighter.CurrentWeapon.GetDamage());
        }
    }
}
