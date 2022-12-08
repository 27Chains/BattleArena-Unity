using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    CreateAssetMenu(
        fileName = "ShieldSO",
        menuName = "Sheilds/Make New Shield",
        order = 0)
]
public class ShieldSO : ScriptableObject
{
    [SerializeField]
    private float absorbtionAmount;

    [SerializeField]
    private GameObject equippedPrefab;

    [SerializeField]
    private float physicalDamageAbsorbtion;

    const string shieldName = "Shield";

    public GameObject CreateInstance(Transform rightHand)
    {
        GameObject shield = Instantiate(equippedPrefab, rightHand);
        shield.name = shieldName;
        return shield;
    }

    public GameObject GetEquippedPrefab()
    {
        return equippedPrefab;
    }

    public float GetPhysicalDamageAbsorbtion()
    {
        return physicalDamageAbsorbtion;
    }
}
