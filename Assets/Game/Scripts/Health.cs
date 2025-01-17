using System;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth = 100;

    [SerializeField]
    private DamageTextSpawner damageTextSpawner;

    [SyncVar]
    private float health;

    [SyncVar]
    public bool isDead;

    public event Action<float, Vector3> OnTakeDamage;

    public event Action OnDie;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        health = maxHealth;
    }

    [Server]
    public void TakeDamage(float damage, Vector3 incomingDirection)
    {
        if (isDead) return;

        health = Mathf.Max(health - damage, 0f);
        TargetOnTakeDamageEvent(base.Owner, health, incomingDirection);
        ObserversDisplayDamage (damage);

        if (health == 0f)
        {
            isDead = true;
            TargetOnDieEvent(base.Owner);
        }
    }

    [TargetRpc]
    private void TargetOnTakeDamageEvent(
        NetworkConnection conn,
        float newHealth,
        Vector3 incomingDirection
    )
    {
        OnTakeDamage?.Invoke(newHealth, incomingDirection);
    }

    [TargetRpc]
    private void TargetOnDieEvent(NetworkConnection conn)
    {
        OnDie?.Invoke();
    }

    [ObserversRpc(IncludeOwner = false)]
    private void ObserversDisplayDamage(float damage)
    {
        damageTextSpawner.SpawnDamageText (damage);
    }

    public float GetHealthPoints()
    {
        return health;
    }

    public float GetMaxHealthPoints()
    {
        return maxHealth;
    }
}
