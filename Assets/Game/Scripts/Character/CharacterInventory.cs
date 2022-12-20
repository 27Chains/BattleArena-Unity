using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class CharacterInventory : NetworkBehaviour
{
    [SerializeField]
    public WeaponSO[] Weapons;

    [SerializeField]
    public Item[] SupportItems;

    public WeaponSO Weapon { get; private set; }

    public Item SupportItem { get; private set; }

    [SerializeField]
    private Transform _rightHandWeaponHolder;

    [SerializeField]
    private Transform _leftHandWeaponHolder;

    // TODO
    // 1. Get user wallet address
    // 2. Get equipped NFTs from the contract
    // 3. Spawn the items
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner) return;
        string walletAddress = "0x0000000000000000000000000000000000000000";
        ServerInitialize (walletAddress);
    }

    [ServerRpc]
    public void ServerInitialize(string walletAddress)
    {
        // check what items are equipped
        Weapon = Weapons[0];
        SupportItem = SupportItems[0];

        // create the items
        GameObject weaponInstance =
            Weapon.CreateInstance(_rightHandWeaponHolder);

        GameObject supportItemInstance =
            SupportItem.CreateInstance(_leftHandWeaponHolder);

        // spawn the items
        Spawn(weaponInstance, base.LocalConnection);
        Spawn(supportItemInstance, base.LocalConnection);

        // send the items to the client
        TargetInitialize(base.Owner, 0, 0);
    }

    [TargetRpc]
    public void TargetInitialize(
        NetworkConnection connection,
        int weaponIndex,
        int supportItemIndex
    )
    {
        Weapon = Weapons[weaponIndex];
        SupportItem = SupportItems[supportItemIndex];
    }
}
