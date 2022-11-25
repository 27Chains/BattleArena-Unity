using FishNet.Object;
using UnityEngine;

public class Fighter : NetworkBehaviour
{
    [SerializeField]
    private Transform rightHandWeaponHolder = null;

    [SerializeField]
    private InputReader inputReader;

    [SerializeField]
    private Weapon currentWeapon = null;

    [HideInInspector]
    public GameObject spawnedWeapon = null;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            GetComponent<Fighter>().enabled = false;
        }
        inputReader.ShowWeaponEvent += HandleSpawn;
    }

    private void HandleSpawn()
    {
        if (spawnedWeapon == null)
        {
            SpawnWeapon(this);
        }
        else
        {
            DespawnWeapon (spawnedWeapon);
        }
    }

    [ServerRpc]
    public void SpawnWeapon(Fighter script)
    {
        GameObject weaponInstance =
            currentWeapon.CreateInstance(rightHandWeaponHolder);
        Spawn (weaponInstance, Owner);
        SetSpawnedWeapon (weaponInstance, script);
    }

    [ObserversRpc]
    public void SetSpawnedWeapon(GameObject spawnedWeapon, Fighter script)
    {
        script.spawnedWeapon = spawnedWeapon;
    }

    [ServerRpc]
    public void DespawnWeapon(GameObject weapon)
    {
        ServerManager.Despawn (weapon);
    }
}
