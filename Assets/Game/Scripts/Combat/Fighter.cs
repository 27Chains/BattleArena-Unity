using FishNet.Object;
using UnityEngine;

public class Fighter : NetworkBehaviour
{
    [SerializeField]
    private Transform _rightHandWeaponHolder = null;

    [SerializeField]
    private InputReader inputReader;

    [SerializeField]
    private Weapon _currentWeapon = null;

    [HideInInspector]
    public GameObject _spawnedWeapon = null;

    public Weapon CurrentWeapon => _currentWeapon;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            GetComponent<Fighter>().enabled = false;
            return;
        }
        inputReader.ShowWeaponEvent += HandleSpawn;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        if (!base.IsOwner) return;
        DespawnWeapon (_spawnedWeapon);
    }

    private void HandleSpawn()
    {
        if (_spawnedWeapon == null)
        {
            SpawnWeapon(this);
        }
        else
        {
            DespawnWeapon (_spawnedWeapon);
        }
    }

    // TODO in the future the weapons will spawn based on the players possession after we load from the database
    [ServerRpc]
    public void SpawnWeapon(Fighter script)
    {
        GameObject weaponInstance =
            _currentWeapon.CreateInstance(_rightHandWeaponHolder);
        Spawn(weaponInstance, base.LocalConnection);
        SetSpawnedWeapon (weaponInstance, script);
    }

    [ObserversRpc]
    public void SetSpawnedWeapon(GameObject spawnedWeapon, Fighter script)
    {
        script._spawnedWeapon = spawnedWeapon;
    }

    // TODO just for testing, user should not despawn weapon on request but need to be able to sheath it
    [ServerRpc]
    public void DespawnWeapon(GameObject weapon)
    {
        ServerManager.Despawn (weapon);
    }
}
