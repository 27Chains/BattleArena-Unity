using UnityEngine;

[
    CreateAssetMenu(
        fileName = "ShieldSO",
        menuName = "Items/Shield/Make New Shield",
        order = 0)
]
public class ShieldSO : Item
{
    [SerializeField]
    private float absorbtionAmount;

    [SerializeField]
    private float physicalDamageAbsorbtion;

    public float GetPhysicalDamageAbsorbtion()
    {
        return physicalDamageAbsorbtion;
    }
}
