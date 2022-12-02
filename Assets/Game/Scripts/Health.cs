using System;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth = 100;

    [SyncVar]
    private float health;

    public bool isDead;

    public event Action OnTakeDamage;

    public event Action OnDie;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        health = maxHealth;
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health = Mathf.Max(health - damage, 0f);
        OnTakeDamage?.Invoke();
        ObserverOnTakeDamageEvent(base.Owner);

        if (health == 0f)
        {
            isDead = true;
            OnDie?.Invoke();
        }
    }

    [TargetRpc]
    private void ObserverOnTakeDamageEvent(NetworkConnection conn)
    {
        OnTakeDamage?.Invoke();
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
