using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float duration = 2f;
    private float damage = 5f;
    private float knockbackPower = 8f;

    private GameObject owner;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner) return;

        IDamageable damageable =
            other.GetComponentInParent<IDamageable>();

        if (damageable == null) return;

        Vector3 dir =
            (other.transform.position - transform.position).normalized;

        DamageInfo info = new DamageInfo(
            damage,
            owner,
            other.ClosestPoint(transform.position),
            dir
        );

        damageable.TakeDamageWithKnockback(info, knockbackPower);
    }
}