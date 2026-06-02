using UnityEngine;

public interface IDamageable
{
    void TakeDamage(DamageInfo damageInfo);
    void TakeDamageWithKnockback(DamageInfo damageInfo, float force);
}