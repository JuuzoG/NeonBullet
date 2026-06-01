using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public float maxHealth;
    public float movementSpeed;
    public float detectionRange;
    public float aggroRange;
    public float attackRange;
    public float attackRadius;
    public float attackDamage;
}
