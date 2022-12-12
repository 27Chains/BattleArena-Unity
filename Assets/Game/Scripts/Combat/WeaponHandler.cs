using FishNet.Object;
using UnityEngine;

public class WeaponHandler : NetworkBehaviour
{
    [SerializeField]
    private DamageCollider weaponCollider;

    // called from animation event
    public void EnableWeapon()
    {
        if (IsServer)
        {
            if (!weaponCollider.isAttacking) return;
            weaponCollider.SetWeaponActive(true);
        }
    }

    // called from animation event
    public void DisableWeapon()
    {
        if (IsServer)
        {
            weaponCollider.SetWeaponActive(false);
        }
    }
}
