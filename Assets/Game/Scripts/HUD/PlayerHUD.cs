using System;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthValue;

    private Health health;

    public void Initialize(Health health)
    {
        UpdateHealth(health.GetHealthPoints());
        health.OnTakeDamage += UpdateHealth;
    }

    private void UpdateHealth(
        float newHealth,
        Vector3 incomingDirection = default
    )
    {
        healthValue.text = newHealth.ToString();
    }
}
