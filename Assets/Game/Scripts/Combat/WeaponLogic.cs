using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    [SerializeField]
    private Collider myCollider;

    [SerializeField]
    private Fighter fighter;

    private List<Collider> alreadyCollidedWith = new List<Collider>();
}
