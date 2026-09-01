using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyOverhaul : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public EnemyStats stats;
    public LootTable lootTable;

    [Header("Combat")]
    public float attackCooldown = 1f;
    public float attackRadius = 90f;

    [Header("Health")]
    public Image healthbar;

    [Header("UI")]
    public GameObject enemyAlert;

    [Header("State")]
    public enemystate currentState;

    private float health;
    private float nextAttackTime;

    private bool attacking;
    private bool isDead;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private Patrolling patrol;


    void Start()
    {
        player = GameManager.instance.player.transform;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        patrol = GetComponent<Patrolling>();

        if (stats != null)
        {
            health = stats.maxHealth;

            if (agent != null)
                agent.speed = stats.movementSpeed;
        }

        if (enemyAlert != null)
            enemyAlert.SetActive(false);


        if (patrol != null)
            patrol.StartPatrol();
    }


    void Update()
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


        // Don't interrupt an attack
        if (attacking)
            return;


        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance > stats.detectionRange)
        {
            enemyAlert.SetActive(false);

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

        if (enemyAlert != null)
            enemyAlert.SetActive(true);


        // Stop patrol
        if (patrol != null)
            patrol.StopPatrol();

        if (distance <= stats.attackRange)
        {
            currentState = enemystate.chasing;

            agent.isStopped = true;

            animator.SetFloat("speed", 0f);


            if (Time.time >= nextAttackTime)
            {
                AttackAnimation();
            }

            return;
        }

        currentState = enemystate.chasing;

        agent.isStopped = false;

        agent.SetDestination(player.position);

        UpdateAnimation();
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

    private void AttackAnimation()
    {
        attacking = true;

        nextAttackTime = Time.time + attackCooldown;

        agent.isStopped = true;

        animator.SetTrigger("attacking");
    }


    // Called by the attack animation
    public void Attack()
    {
        if (isDead)
            return;

        if (player == null)
            return;

        if (health <= 0)
            return;


        Vector3 direction =
            player.position - transform.position;

        float distance = direction.magnitude;


        // Too far away
        if (distance > stats.attackRange)
            return;


        // Make sure enemy is actually facing player
        if (Vector3.Angle(
            transform.forward,
            direction
        ) > attackRadius * 0.5f)
        {
            return;
        }


        Player playerScript =
            player.GetComponent<Player>();

        if (playerScript != null)
        {
            playerScript.GainHealth(-stats.attackDamage);
        }
    }


    // Called by the attack animation
    public void EndAttack()
    {
        attacking = false;

        if (isDead)
            return;

        if (CanUseAgent())
            agent.isStopped = false;
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
        {
            Die();
        }
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
        {
            agent.isStopped = false;
        }
    }
    public void Die()
    {
        if (isDead)
            return;


        isDead = true;

        health = 0;

        attacking = true;


        // Stop everything
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


        // Turn off alert
        if (enemyAlert != null)
            enemyAlert.SetActive(false);


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

    void LateUpdate()
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

public enum enemystate
{
    patrolling,
    chasing
}
