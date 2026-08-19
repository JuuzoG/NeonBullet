using UnityEngine;

public class LaserWall : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float pushForce = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(new DamageInfo(damage));
        }

        PlayerCharacterController player = collision.gameObject.GetComponentInParent<PlayerCharacterController>();

        if (player != null)
        {
            Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
            player.Knockback(pushDirection, pushForce);
        }
    }
}