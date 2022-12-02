using FishNet.Object;
using UnityEngine;

public class WeaponHandler : NetworkBehaviour
{
    [SerializeField]
    private GameObject weaponLogic;

    private bool unlocked;

    [ServerRpc]
    public void UnlockWeapon()
    {
        unlocked = true;
    }

    [ServerRpc]
    public void LockWeapon()
    {
        unlocked = false;
        DisableWeapon();
    }

    public void EnableWeapon()
    {
        if (IsServer)
        {
            if (!unlocked) return;
            weaponLogic.SetActive(true);
        }
    }

    public void DisableWeapon()
    {
        if (IsServer)
        {
            weaponLogic.SetActive(false);
        }
    }
}
