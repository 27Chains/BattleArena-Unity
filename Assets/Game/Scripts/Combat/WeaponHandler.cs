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

    private void Update()
    {
        if (weaponCollider.activeInHierarchy)
        {
            Debug.Log("Weapon is active");
        }
        else
        {
            Debug.Log("Weapon is not active");
        }
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
            Debug.Log("Weapon is enabled");
            weaponCollider.SetActive(true);
            Debug.Log(weaponCollider.activeInHierarchy);
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
