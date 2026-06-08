using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public PlayerStats stats;
    public float health;
    private float energy;
    public int munition = 10;

    [Header("Additionals")]
    public GameObject projectilePrefab;
    public HubUI ui;

    void Awake()
    {
        energy = stats.maxEnergy;
        health = stats.maxHealth;

        GameManager.instance.player = this;

        if (ui != null)
        {
            ui.UpdateHealth((int)health, (int)stats.maxHealth);
            ui.UpdateEnergy((int)energy, (int)stats.maxEnergy);
        }
    }

    void Update()
    {
        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;

        if (Input.GetKeyDown(KeyCode.Mouse0) && munition > 0)
        {
            Vector3 position =
                transform.position +
                transform.forward +
                Vector3.up;

            GameObject proj =
                Instantiate(projectilePrefab, position, transform.rotation);

            Projectile p = proj.GetComponent<Projectile>();
            if (p != null)
                p.SetOwner(gameObject);

            munition--;
        }

        GainEnergy(stats.energyRecoverRate * Time.deltaTime);
    }

    public void GainMunition(int amount)
    {
        munition += amount;
    }

    public void GainHealth(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, stats.maxHealth);

        if (ui != null)
        {
            ui.UpdateHealth((int)health, (int)stats.maxHealth);
        }

        if (health <= 0)
        {
            GameManager.instance.state = GameStates.GameOver;
            Time.timeScale = 0;
        }
    }

    public float GainEnergy(float amount)
    {
        energy += amount;
        energy = Mathf.Clamp(energy, 0, stats.maxEnergy);

        if (ui != null)
        {
            ui.UpdateEnergy((int)energy, (int)stats.maxEnergy);
        }

        return energy;
    }

    public void TakeDamage(DamageInfo info)
    {
        GainHealth(-info.damage);
    }

    public void TakeDamageWithKnockback(DamageInfo info, float force)
    {
        GainHealth(-info.damage);
    }
}