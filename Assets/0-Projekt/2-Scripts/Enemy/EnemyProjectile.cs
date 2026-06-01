using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime); // destroy after some time
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check for anything that implements IDamageable in parent
        if (other.GetComponentInParent<IDamageable>() is IDamageable dmg)
        {
            dmg.TakeDamage(damage);
        }

        Destroy(gameObject); // destroy projectile on impa
    }
}