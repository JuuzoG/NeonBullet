using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyStats stats;
    public LootTable lootTable;
    private Player player;
    private NavMeshAgent agent;
    private Animator animator;
    private float health;
    public bool attacking;

    void Start()
    {
        health = stats.maxHealth;
        player = GameManager.instance.player;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;
        if (health <= 0) return;
        if (attacking) return;

        animator.SetFloat("speed", agent.velocity.magnitude);
        Vector3 direction = player.transform.position - transform.position;

        if (direction.magnitude > stats.detectionRange) return;
        if (direction.magnitude <= stats.aggroRange)
        {
            attacking = true;
            animator.SetTrigger("attacking");
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);            
        }        
    }

    public void RecieveHit(float damage, Vector3 knockbackDirection, float knockbackPower)
    {
        health -= damage;
        agent.velocity = knockbackDirection*knockbackPower;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        animator.SetTrigger("die");
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
        Destroy(agent);
        Instantiate(lootTable.GetDrop(), transform.position, Quaternion.identity);
    }

    public void Attack()
    {
        Vector3 dir = player.transform.position - transform.position;
        if (health <= 0) return;
        if (dir.magnitude > stats.attackRange) return;
        if (Vector3.Angle(transform.forward, dir) > stats.attackRadius * 0.5f) return;
        player.GainHealth(stats.attackDamage*-1);
    }

    public void EndAttack()
    {
        attacking = false;
    }

}
