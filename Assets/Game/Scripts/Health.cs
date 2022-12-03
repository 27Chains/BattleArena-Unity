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

    public event Action<float> OnTakeDamage;

    public event Action OnDie;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        health = maxHealth;
    }

    [Server]
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health = Mathf.Max(health - damage, 0f);
        TargetOnTakeDamageEvent(base.Owner, damage);

        if (health == 0f)
        {
            isDead = true;
            OnDie?.Invoke();
        }
    }

    [TargetRpc]
    private void TargetOnTakeDamageEvent(NetworkConnection conn, float damage)
    {
        OnTakeDamage?.Invoke(damage);
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
