using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float duration = 2f;
    private float damage = 5f;
    private float knockbackPower = 8f;
    private float radius = 3f;

    private GameObject owner;

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }
    private bool hasExploded;

    void Start()
    {
        Invoke(nameof(Explode), 0.05f); // slight delay so spawn is stable
        Destroy(gameObject, duration);
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (var hit in hits)
        {
            IDamageable dmg = hit.GetComponentInParent<IDamageable>();
            if (dmg == null) continue;

            MonoBehaviour mb = dmg as MonoBehaviour;

            if (mb != null && owner != null && mb.gameObject == owner)
                continue;

            Vector3 dir = (hit.transform.position - transform.position).normalized;

            DamageInfo info = new DamageInfo(
                damage,
                owner,
                hit.ClosestPoint(transform.position),
                dir
            );

            dmg.TakeDamageWithKnockback(info, knockbackPower);
        }
    }
}