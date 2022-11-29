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

    [ServerRpc]
    public void EnableWeapon()
    {
        if (!unlocked) return;
        weaponLogic.SetActive(true);
    }

    [ServerRpc]
    public void DisableWeapon()
    {
        weaponLogic.SetActive(false);
    }
}
