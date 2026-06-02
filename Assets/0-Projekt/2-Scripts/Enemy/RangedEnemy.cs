using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RangedEnemy : MonoBehaviour, IDamageable
{
    [Header("Ranges")]
    [SerializeField] private float aggroRange = 15f;
    [SerializeField] private float attackRange = 10f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;

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
    private bool isDead;

    private void Start()
    {
        player = GameManager.instance.player;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        health = stats.maxHealth;
        agent.speed = stats.movementSpeed;
    }

    private void Update()
    {
        if (isDead) return;

        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;

        if (!CanUseAgent()) return;
        if (player == null) return;

        animator.SetFloat("speed", agent.velocity.magnitude);

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > aggroRange)
        {
            agent.isStopped = true;
            return;
        }

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.isStopped = true;

            Vector3 look = player.transform.position;
            look.y = transform.position.y;
            transform.LookAt(look);

            TryShoot();
        }
    }

    private void TryShoot()
    {
        if (isDead) return;
        if (Time.time < lastAttackTime + attackCooldown) return;
        if (projectilePrefab == null || shootPoint == null) return;

        lastAttackTime = Time.time;

        Vector3 dir = (player.transform.position - shootPoint.position).normalized;

        GameObject proj = Instantiate(
            projectilePrefab,
            shootPoint.position,
            Quaternion.LookRotation(dir)
        );

        EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();
        if (ep != null)
            ep.SetOwner(gameObject);

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)

        if (audioSource && shootSound)
            audioSource.PlayOneShot(shootSound);
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

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        health = 0;

        if (animator != null)
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

    private bool CanUseAgent()
    {
        return agent != null && agent.enabled && agent.isOnNavMesh;
    }
}