using System;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthValue;

    private Health health;

    public void Initialize(Player player)
    {
        health = player.GetComponent<Health>();
        UpdateHealth(health.GetHealthPoints());
        health.OnTakeDamage += UpdateHealth;
    }

    private void UpdateHealth(float newHealth)
    {
        healthValue.text =
            System
                .String
                .Format("{0}/{1}", newHealth, health.GetMaxHealthPoints());
    }
}
