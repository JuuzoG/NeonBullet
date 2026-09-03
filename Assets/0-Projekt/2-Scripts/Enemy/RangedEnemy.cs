using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class RangedEnemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public EnemyStats stats;
    public LootTable lootTable;

    [Header("State")]
    public enemystate currentState;

    [Header("Ranges")]
    [SerializeField] private float attackRange = 10f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;

    [Header("Health")]
    public Image healthbar;

    private NavMeshAgent agent;
    private Animator animator;
    private Player player;
    private Patrolling patrol;

    private float health;
    private float lastAttackTime;

    private bool isDead;

    private void Start()
    {
        player = GameManager.instance.player;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        patrol = GetComponent<Patrolling>();

        if (stats != null)
        {
            health = stats.maxHealth;

            if (agent != null)
                agent.speed = stats.movementSpeed;
        }

        // Start patrolling
        if (patrol != null)
            patrol.StartPatrol();
    }

    private void Update()
    {
        if (isDead)
            return;

        if (player == null)
            return;

        // Don't do anything during these states
        if (GameManager.instance.state == GameStates.GameOver)
            return;

        if (GameManager.instance.state == GameStates.paused)
            return;

        if (GameManager.instance.state == GameStates.hacking)
            return;

        if (!CanUseAgent())
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        if (distance > stats.detectionRange)
        {
            currentState = enemystate.patrolling;

            // Give navigation back to patrol
            if (patrol != null && !patrol.IsPatrolling)
            {
                patrol.StartPatrol();
            }

            agent.isStopped = false;

            UpdateAnimation();

            return;
        }

      
        currentState = enemystate.chasing;

        // Stop patrol
        if (patrol != null)
            patrol.StopPatrol();

        if (distance > attackRange)
        {
            agent.isStopped = false;

            agent.SetDestination(
                player.transform.position
            );

            UpdateAnimation();

            return;
        }

        agent.isStopped = true;

        UpdateAnimation();

        FacePlayer();

        TryShoot();

        UpdateHealthBar();
    }

    private void UpdateAnimation()
    {
        if (animator == null)
            return;

        animator.SetFloat(
            "speed",
            agent.velocity.magnitude
        );
    }

    private void FacePlayer()
    {
        if (player == null)
            return;

        Vector3 look = player.transform.position;

        // Don't tilt enemy up/down
        look.y = transform.position.y;

        Vector3 direction = look - transform.position;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 10f
            );
        }
    }

    private void TryShoot()
    {
        if (isDead)
            return;

        if (player == null)
            return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        if (projectilePrefab == null || shootPoint == null)
            return;

        lastAttackTime = Time.time;

        // Aim above player's root position
        Vector3 target =
            player.transform.position +
            Vector3.up * 1.2f;

        Vector3 dir =
            (target - shootPoint.position).normalized;

        GameObject proj = Instantiate(
            projectilePrefab,
            shootPoint.position,
            Quaternion.LookRotation(dir)
        );

        EnemyProjectile ep =
            proj.GetComponent<EnemyProjectile>();

        if (ep != null)
            ep.SetOwner(gameObject);

        Rigidbody rb =
            proj.GetComponent<Rigidbody>();

        if (rb != null)
            rb.linearVelocity = dir * 15f;

        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound);
    }

    public void TakeDamage(DamageInfo info)
    {
        ApplyDamage(info.damage);
    }

    public void TakeDamageWithKnockback(
        DamageInfo info,
        float force)
    {
        ApplyDamage(info.damage);

        if (isDead)
            return;

        if (CanUseAgent())
        {
            agent.isStopped = true;

            agent.velocity =
                info.direction * force;

            StartCoroutine(ResumeAgent());
        }
    }

    private void ApplyDamage(float damage)
    {
        if (isDead)
            return;

        health -= damage;

        health = Mathf.Max(
            health,
            0
        );

        UpdateHealthBar();

        if (health <= 0)
            Die();
    }

    private void UpdateHealthBar()
    {
        if (healthbar == null)
            return;

        if (stats == null)
            return;

        healthbar.fillAmount =
            health / stats.maxHealth;
    }

    private System.Collections.IEnumerator ResumeAgent()
    {
        yield return new WaitForSeconds(0.2f);

        if (isDead)
            yield break;

        if (CanUseAgent())
            agent.isStopped = false;
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        health = 0;

        StopAllCoroutines();

        // Death animation
        if (animator != null)
            animator.SetTrigger("die");

        // Disable navigation
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.enabled = false;
        }

        // Stop patrol
        if (patrol != null)
            patrol.StopPatrol();

        // Remove physics
        Rigidbody rb =
            GetComponent<Rigidbody>();

        if (rb != null)
            Destroy(rb);

        // Remove collider
        Collider col =
            GetComponent<Collider>();

        if (col != null)
            Destroy(col);

        // Remove health bar
        if (healthbar != null)
        {
            if (healthbar.transform.parent != null)
            {
                Destroy(
                    healthbar.transform.parent.gameObject
                );
            }
        }

        // Change tag
        gameObject.tag = "none";

        // Drop loot
        if (lootTable != null)
        {
            GameObject loot =
                lootTable.GetDrop();

            if (loot != null)
            {
                Instantiate(
                    loot,
                    transform.position,
                    Quaternion.identity
                );
            }
        }

        // Disable this script
        this.enabled = false;
    }

    private bool CanUseAgent()
    {
        return agent != null &&
               agent.enabled &&
               agent.isOnNavMesh;
    }

    private void LateUpdate()
    {
        if (isDead)
            return;

        if (healthbar == null)
            return;

        if (healthbar.transform.parent == null)
            return;

        if (Camera.main == null)
            return;

        healthbar.transform.parent.rotation =
            Camera.main.transform.rotation;
    }
}
