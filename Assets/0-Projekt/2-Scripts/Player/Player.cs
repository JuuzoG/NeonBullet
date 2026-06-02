using UnityEngine;

public class Player : MonoBehaviour
{
    public float health;
    private float energy;
    public PlayerStats stats;
    public int munition = 10;
    public GameObject projectilePrefab;

    void Awake()
    {
        energy = stats.maxEnergy;
        health = stats.maxHealth;
        GameManager.instance.player = this; 
    }

    void Update()
    {
        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;

        if (Input.GetKeyDown(KeyCode.Mouse0) && munition > 0)
        {
            Vector3 position = transform.position + transform.forward *1f + Vector3.up;
            Instantiate(projectilePrefab, position, transform.rotation);
            munition--;
        }   
        GainEnergy(stats.energyRecoverRate*Time.deltaTime);   
    }

    public void GainMunition(int amount)
    {
        munition += amount;
    }

    public void GainHealth(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health,0, stats.maxHealth);

        if (health == 0)
        {
            GameManager.instance.state = GameStates.GameOver;
            Time.timeScale = 0;
        }
    }

    public float GainEnergy(float amount)
    {
        energy += amount;
        energy = Mathf.Clamp(energy,0, stats.maxEnergy);
        return energy;
    }
}
