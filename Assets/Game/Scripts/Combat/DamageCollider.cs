using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class DamageCollider : NetworkBehaviour
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

        if (other.tag == "Player")
        {
            Health health = other.GetComponent<Health>();
            BlockingCollider shield =
                other.transform.GetComponentInChildren<BlockingCollider>();
            float currentWeaponDamage = fighter.CurrentWeapon.GetDamage();
            if (shield.IsBlocking)
            {
                float blockDamageReduction =
                    fighter.CurrentShield.GetPhysicalDamageAbsorbtion();
                currentWeaponDamage -=
                    (currentWeaponDamage * blockDamageReduction) / 100;

                health.TakeDamage (currentWeaponDamage);
                return;
            }
            if (health != null)
            {
                health.TakeDamage (currentWeaponDamage);
            }
        }
    }
}
