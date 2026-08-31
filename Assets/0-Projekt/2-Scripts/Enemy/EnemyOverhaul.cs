using UnityEngine;
using UnityEngine.AI;

public class EnemyOverhaul : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    private bool attacking;
    private float nextAttackTime;

    void Start()
    {
        player = GameManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Player is too far away
        if (distance > detectionRange)
        {
            agent.isStopped = true;
            animator.SetFloat("speed", 0);
            return;
        }

        // Player is in attack range
        if (distance <= attackRange)
        {
            agent.isStopped = true;
            animator.SetFloat("speed", 0);

            if (!attacking && Time.time >= nextAttackTime)
            {
                AttackAnimation();
            }

            return;
        }

        // Chase player
        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator.SetFloat("speed", agent.velocity.magnitude);
    }

    void AttackAnimation()
    {
        attacking = true;
        nextAttackTime = Time.time + attackCooldown;

        animator.SetTrigger("attacking");
    }

    // Call from attack animation
    public void Attack()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            player.GetComponent<Player>().GainHealth(-attackDamage);
        }
    }

    // Call this at end of attack animation
    public void EndAttack()
    {
        attacking = false;
    }
}

