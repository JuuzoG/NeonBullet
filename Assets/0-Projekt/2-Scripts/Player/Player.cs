using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public PlayerStats stats;
    public float health;
    private float energy;
    private int munition = 10;

    [Header("Additionals")]
    public GameObject projectilePrefab;
    public UI ui;

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

            Instantiate(projectilePrefab, position, transform.rotation);

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
}