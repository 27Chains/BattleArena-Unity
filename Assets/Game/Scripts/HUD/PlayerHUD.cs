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
        UpdateHealth();
        health.OnTakeDamage += UpdateHealth;
    }

    private void UpdateHealth()
    {
        healthValue.text =
            System
                .String
                .Format("{0}/{1}",
                health.GetHealthPoints(),
                health.GetMaxHealthPoints());
    }
}
