using FishNet.Object;
using UnityEngine;

public class WeaponHandler : NetworkBehaviour
{
    [SerializeField]
    private GameObject weaponCollider;

    private bool unlocked;

    [Server]
    public void UnlockWeapon()
    {
        unlocked = true;
    }

    [Server]
    public void LockWeapon()
    {
        unlocked = false;
        DisableWeapon();
    }

    // called from animation event
    public void EnableWeapon()
    {
        if (IsServer)
        {
            if (!unlocked) return;
            weaponCollider.SetActive(true);
        }
    }

    // called from animation event
    public void DisableWeapon()
    {
        if (IsServer)
        {
            weaponCollider.SetActive(false);
        }
    }
}
