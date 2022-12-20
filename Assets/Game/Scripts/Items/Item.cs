using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField]
    private GameObject equippedPrefab;

    public GameObject CreateInstance(Transform rightHand)
    {
        GameObject weapon = Instantiate(equippedPrefab, rightHand);
        return weapon;
    }

    public GameObject GetEquippedPrefab()
    {
        return equippedPrefab;
    }
}
