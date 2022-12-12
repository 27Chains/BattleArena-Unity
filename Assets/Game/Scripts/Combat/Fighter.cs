using FishNet.Object;
using UnityEngine;

public class Fighter : NetworkBehaviour
{
    [SerializeField]
    private Transform _rightHandWeaponHolder = null;

    [SerializeField]
    private Transform _leftHandWeaponHolder = null;

    [SerializeField]
    private InputReader inputReader;

    [SerializeField]
    private WeaponSO _currentWeapon = null;

    [SerializeField]
    private ShieldSO _currentShield = null;

    [HideInInspector]
    public GameObject _spawnedWeapon = null;

    [HideInInspector]
    public GameObject _spawnedShield = null;

    public WeaponSO CurrentWeapon => _currentWeapon;

    public ShieldSO CurrentShield => _currentShield;

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

    public override void OnStopClient()
    {
        base.OnStopClient();
        if (base.IsOwner) DespawnEquipment(this);
    }

    private void HandleSpawn()
    {
        if (!IsOwner) return;
        if (_spawnedWeapon == null)
        {
            SpawnEquipment(this);
        }
    }

    // TODO in the future the weapons will spawn based on the players possession after we load from the database
    [ServerRpc]
    public void SpawnEquipment(Fighter script)
    {
        GameObject weaponInstance =
            _currentWeapon.CreateInstance(_rightHandWeaponHolder);

        GameObject shieldInstance =
            _currentShield.CreateInstance(_leftHandWeaponHolder);

        Spawn(weaponInstance, base.LocalConnection);
        Spawn(shieldInstance, base.LocalConnection);
    }

    [ServerRpc]
    public void DespawnEquipment(Fighter script)
    {
        Despawn(script._spawnedWeapon);
        Despawn(script._spawnedShield);
    }
}
