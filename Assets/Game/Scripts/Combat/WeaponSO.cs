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
    private float minDamage;

    [SerializeField]
    private float maxDamage;

    [SerializeField]
    private float knockbackForce;

    [SerializeField]
    private float WeaponForce;

    [SerializeField]
    public string[] AttackAnimations;

    [SerializeField]
    public float[] ComboAttackTime;

    [SerializeField]
    public float[] ComboAttackWindow;

    public float GetDamage()
    {
        return Mathf.RoundToInt(Random.Range(minDamage, maxDamage));
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
