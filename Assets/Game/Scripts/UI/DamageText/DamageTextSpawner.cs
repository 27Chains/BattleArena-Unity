using FishNet.Object;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField]
    private DamageText damageTextPrefab;

    public void SpawnDamageText(float damage)
    {
        DamageText damageTextInstance =
            Instantiate(damageTextPrefab, transform);
        damageTextInstance.SetValue (damage);
    }
}
