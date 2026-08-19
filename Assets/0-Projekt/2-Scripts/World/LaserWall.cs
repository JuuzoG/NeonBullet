using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class LaserWall : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(new DamageInfo(damage));
        }
    }
}