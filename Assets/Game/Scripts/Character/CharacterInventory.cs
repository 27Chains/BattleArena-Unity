using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    [field: SerializeField]
    public WeaponSO Weapon { get; private set; }

    [field: SerializeField]
    public Item SupportItem { get; private set; }

    private bool isTwoHanded;
}
