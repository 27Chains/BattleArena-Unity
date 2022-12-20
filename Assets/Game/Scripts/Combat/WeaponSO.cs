using UnityEngine;

[
    CreateAssetMenu(
        fileName = "WeaponSO",
        menuName = "Items/Weapons/Make New Weapon",
        order = 0)
]
// TODO what should this component include? if item is upgradeable how to approach it, do we need to make a new weapon or just upgrade the existing one?
public class WeaponSO : Item
{
    [SerializeField]
    private float weaponDamage;

    [SerializeField]
    private float weaponRange;

    [SerializeField]
    private float knockbackForce;

    [SerializeField]
    private float WeaponForce;

    [SerializeField]
    private GameObject equippedPrefab;

    [SerializeField]
    public string[] AttackAnimations;

    [SerializeField]
    public float[] ComboAttackTime;

    [SerializeField]
    public float[] ComboAttackWindow;

    const string weaponName = "Weapon";

    public GameObject CreateInstance(Transform rightHand)
    {
        GameObject weapon = Instantiate(equippedPrefab, rightHand);
        weapon.name = weaponName;
        return weapon;
    }

    public float GetDamage()
    {
        return weaponDamage;
    }

    public GameObject GetEquippedPrefab()
    {
        return equippedPrefab;
    }

    public float GetRange()
    {
        return weaponRange;
    }

    public float GetWeaponForce()
    {
        return WeaponForce;
    }

    public float GetKnockbackForce()
    {
        return knockbackForce;
    }
}
