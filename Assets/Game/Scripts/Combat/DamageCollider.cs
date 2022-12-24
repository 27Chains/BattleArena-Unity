using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class DamageCollider : NetworkBehaviour
{
    [SerializeField]
    private Collider myCollider;

    [SerializeField]
    private float weaponLength = 1.5f;

    private bool blocked;

    [SerializeField]
    private GameObject raycastOrigin;

    private List<Collider> alreadyCollidedWith = new List<Collider>();

    public bool isEnabled;

    private CharacterInventory inventory;

    public override void OnStartServer()
    {
        base.OnStartServer();
        inventory = GetComponentInParent<CharacterInventory>();
    }

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
                    float currentWeaponDamage = inventory.Weapon.GetDamage();

                    Character enemy = hit.collider.GetComponent<Character>();
                    bool enemyBlocking =
                        enemy.BlockingCollider.blockingCollider.enabled;

                    if (enemyBlocking)
                    {
                        currentWeaponDamage =
                            Mathf
                                .RoundToInt(currentWeaponDamage *
                                ((ShieldSO) enemy.Inventory.SupportItem)
                                    .GetPhysicalDamageAbsorbtion() /
                                100);
                    }
                    hit
                        .collider
                        .GetComponent<Health>()
                        .TakeDamage(currentWeaponDamage,
                        myCollider.transform.position);
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
