using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI damageText;

    public void SetValue(float value)
    {
        damageText.text = value.ToString();
    }

    public void DestroyText()
    {
        Destroy (gameObject);
    }
}
