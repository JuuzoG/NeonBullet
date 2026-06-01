using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float duration = 2f;
    private float damage = 5f;
    private float knockbackPower = 8f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter(Collider other)
    {
        IDamageable damageable =
            other.GetComponentInParent<IDamageable>();

        if (damageable == null) return;

        Vector3 dir =
            (other.transform.position - transform.position).normalized;

        damageable.TakeDamageWithKnockback(damage, dir, knockbackPower); //e
    }
}