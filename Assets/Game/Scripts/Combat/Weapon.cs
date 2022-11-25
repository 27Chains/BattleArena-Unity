using UnityEngine;

[
    CreateAssetMenu(
        fileName = "Weapon",
        menuName = "Weapons/Make New Weapon",
        order = 0)
]
public class Weapon : ScriptableObject
{
    [SerializeField]
    private float weaponDamage;

    [SerializeField]
    private float weaponRange;

    [SerializeField]
    private GameObject equippedPrefab;

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
}
