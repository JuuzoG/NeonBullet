using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Needs")]
    public EnemyStats stats;
    public LootTable lootTable;
    private Player player;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Stats + Bools")]
    private float health;
    public bool attacking;
    private bool isDead;


    void Start()
    {
        health = stats.maxHealth;

        player = GameManager.instance.player;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        if (agent != null)
            agent.speed = stats.movementSpeed;
    }

    void Update()
    {
        if (isDead) return;

        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;
        if (health <= 0) return;
        if (attacking) return;

        if (!CanUseAgent()) return;

        animator.SetFloat("speed", agent.velocity.magnitude);

        Vector3 direction = player.transform.position - transform.position;
        float distance = direction.magnitude;

        if (distance > stats.detectionRange) return;

        if (distance <= stats.aggroRange)
        {
            attacking = true;
            animator.SetTrigger("attacking");

            if (CanUseAgent())
                agent.isStopped = true;
        }
        else
        {
            if (CanUseAgent())
            {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
            }
        }
    }

    public void TakeDamage(DamageInfo info)
    {
        ApplyDamage(info.damage);
    }

    public void TakeDamageWithKnockback(DamageInfo info, float force)
    {
        ApplyDamage(info.damage);

        if (CanUseAgent())
        {
            agent.isStopped = true;
            agent.velocity = info.direction * force;
            StartCoroutine(ResumeAgent());
        }
    }


    private void ApplyDamage(float damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
            Die();
    }

    private System.Collections.IEnumerator ResumeAgent()
    {
        yield return new WaitForSeconds(0.2f);

        if (isDead) yield break;

        if (CanUseAgent())
            agent.isStopped = false;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        health = 0;
        attacking = true;

        animator.SetTrigger("die");

        StopAllCoroutines();

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.enabled = false;
        }

        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());

        GameObject loot = lootTable.GetDrop();
        if (loot != null)
            Instantiate(loot, transform.position, Quaternion.identity);

        this.enabled = false;
    }


    public void Attack()
    {
        if (isDead) return;

        Vector3 dir = player.transform.position - transform.position;

        if (health <= 0) return;
        if (dir.magnitude > stats.attackRange) return;
        if (Vector3.Angle(transform.forward, dir) > stats.attackRadius * 0.5f) return;

        player.GainHealth(-stats.attackDamage);
    }

    public void EndAttack()
    {
        attacking = false;
    }

    private bool CanUseAgent()
    {
        return agent != null && agent.enabled && agent.isOnNavMesh;
    }


}