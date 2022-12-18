using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class DamageCollider : NetworkBehaviour
{
    [SerializeField]
    private Collider myCollider;

    [SerializeField]
    private float weaponLength = 1.5f;

    [SerializeField]
    private GameObject raycastOrigin;

    [SerializeField]
    private Fighter fighter;

    private List<Collider> alreadyCollidedWith = new List<Collider>();

    public bool isEnabled;

    private void Update()
    {
        if (!isEnabled) return;
        for (int i = 0; i < 15; i++)
        {
            float angle =
                raycastOrigin.transform.eulerAngles.y + i * 90 / 15 - 45;
            RaycastHit hit;
            if (
                Physics
                    .Raycast(raycastOrigin.transform.position,
                    Quaternion.Euler(0, angle, 0) * Vector3.forward,
                    out hit,
                    weaponLength)
            )
            {
                if (hit.collider == myCollider) return;
                if (alreadyCollidedWith.Contains(hit.collider)) return;
                alreadyCollidedWith.Add(hit.collider);
                if (hit.collider.CompareTag("Player"))
                {
                    float currentWeaponDamage =
                        fighter.CurrentWeapon.GetDamage();
                    hit
                        .collider
                        .GetComponent<Health>()
                        .TakeDamage(currentWeaponDamage,
                        fighter.transform.position);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < 15; i++)
        {
            float angle =
                raycastOrigin.transform.eulerAngles.y + i * 90 / 15 - 45;
            Gizmos
                .DrawRay(raycastOrigin.transform.position,
                Quaternion.Euler(0, angle, 0) * Vector3.forward * weaponLength);
        }
    }

    [Server]
    public void DisableCollider()
    {
        isEnabled = false;
        alreadyCollidedWith.Clear();
    }

    [Server]
    public void SetWeaponActive(bool active)
    {
        isEnabled = active;
        alreadyCollidedWith.Clear();
    }
}
