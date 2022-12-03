using FishNet.Object;
using UnityEngine;

public class DamageTextSpawner : NetworkBehaviour
{
    [SerializeField]
    private DamageText damageTextPrefab;

    Health health;

    public void Initialize(Player player)
    {
        health = player.GetComponent<Health>();
        health.OnTakeDamage += SpawnDamageText;
    }

    [ObserversRpc(IncludeOwner = false)]
    public void SpawnDamageText(float damage)
    {
        DamageText damageTextInstance =
            Instantiate(damageTextPrefab, transform);
    }
}
