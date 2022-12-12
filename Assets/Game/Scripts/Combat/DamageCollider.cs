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

    public bool isAttacking;

    private bool isEnabled;

    public void ClearCollisionList()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if (!isAttacking) return;
        if (!isEnabled) return;
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

                health
                    .TakeDamage(currentWeaponDamage,
                    fighter.transform.position);
                return;
            }
            if (health != null)
            {
                health
                    .TakeDamage(currentWeaponDamage,
                    fighter.transform.position);
            }
        }
    }

    [Server]
    public void EnableCollider()
    {
        isAttacking = true;
    }

    [Server]
    public void DisableCollider()
    {
        isAttacking = false;
        isEnabled = false;
    }

    [Server]
    public void SetWeaponActive(bool active)
    {
        isEnabled = active;
    }
}
