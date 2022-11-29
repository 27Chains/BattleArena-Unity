using System;
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

    private void Start()
    {
        health = maxHealth;
    }

    [ServerRpc]
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health = Mathf.Max(health - damage, 0f);
        OnTakeDamage?.Invoke();

        if (health == 0f)
        {
            isDead = true;
            OnDie?.Invoke();
        }
    }
}
