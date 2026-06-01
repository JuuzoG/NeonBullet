using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);
    void TakeDamageWithKnockback(float damage, Vector3 dir, float force);
}