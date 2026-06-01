using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RangedEnemy : MonoBehaviour
{
    [Header("Ranges")]
    [SerializeField] private float aggroRange = 15f;
    [SerializeField] private float attackRange = 10f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float projectileSpeed = 15f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;

    [Header("Stats")]
    public EnemyStats stats;
    public LootTable lootTable;

    private NavMeshAgent agent;
    private Animator animator;
    private Player player;

    private float health;
    private float lastAttackTime;

    private void Start()
    {
        if (GameManager.instance != null)
        {
            player = GameManager.instance.player;
        }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        if (stats != null)
        {
            health = stats.maxHealth;
            agent.speed = stats.movementSpeed;
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        // Player too far away
        if (distance > aggroRange)
        {
            agent.isStopped = true;
            return;
        }

        // Chase until within attack range
        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);

            if (animator != null)
            {
                animator.SetBool("Moving", true);
            }
        }
        else
        {
            // Stop and attack
            agent.isStopped = true;

            Vector3 lookPosition = player.transform.position;
            lookPosition.y = transform.position.y;

            transform.LookAt(lookPosition);

            if (animator != null)
            {
                animator.SetBool("Moving", false);
            }

            TryShoot();
        }
    }

    private void TryShoot()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        if (projectilePrefab == null || shootPoint == null)
            return;

        Vector3 direction =
            (player.transform.position - shootPoint.position).normalized;

        GameObject projectile = Instantiate(
            projectilePrefab,
            shootPoint.position,
            Quaternion.LookRotation(direction)
        );

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        Debug.Log($"{gameObject.name} fired a projectile!");
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Add loot drops here if needed

        Destroy(gameObject);
    }
}