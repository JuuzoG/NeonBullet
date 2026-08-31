using System.Collections;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public PlayerStats stats;
    public float health;
    private float energy;
    public int munition = 10;

    [Header("Input")]
    public KeyCode shot;
    public KeyCode E;
    public KeyCode Inventar;
    public KeyCode Q;
    public KeyCode Menu;

    [Header("Additionals")]
    public GameObject projectilePrefab;
    public HubUI ui;
    public GameObject GameOverScreen;
    [Header("Weapons")]
    private Railgun railgun;

    [Header("Rifle Settings")]
    public int rifleShotCount = 5;
    public float rifleSpreadAngle = 5f;
    public float rifleSpawnInterval = 0.05f;

    [Header("Unlocked")]
    public bool Rifle = true;
    public bool Railgun = true;

    void Start()
    {
        GameOverScreen.SetActive(false);
        railgun = GetComponent<Railgun>();
    }

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
        if (GameManager.instance.state == GameStates.inventory) return;

        if (Input.GetKeyDown(shot) && munition > 0)
        {
            switch (GameManager.instance.WeaponSelect.CurrentWeaponIndex)
            {
                case 0:
                    Vector3 position = new Vector3(transform.position.x,transform.position.y+1.5f,transform.position.z);
                    GameObject proj = Instantiate(projectilePrefab, position, transform.rotation);
                    Projectile p = proj.GetComponent<Projectile>();
                    if (p != null) p.SetOwner(gameObject);
                    munition--;
                    break;
                case 1:
                    if (Railgun)
                    railgun.Fire();
                    break;
                case 2:
                    if (Rifle)
                    StartCoroutine(FireRifle());
                    break;
            }
        }
        
        GainEnergy(stats.energyRecoverRate * Time.deltaTime);
    }

    IEnumerator FireRifle()
    {
        if (munition <= 0) yield break;
        

        int shotsToFire = Mathf.Min(munition, rifleShotCount);
        munition -= shotsToFire;

        Vector3 basePosition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);

        for (int i = 0; i < shotsToFire; i++)
        {
            float angleOffset = (i - (shotsToFire - 1) / 2f) * rifleSpreadAngle;
            Quaternion rotation = transform.rotation * Quaternion.Euler(0, 0, angleOffset);

            GameObject proj = Instantiate(projectilePrefab, basePosition, rotation);
            Projectile p = proj.GetComponent<Projectile>();
            if (p != null) p.SetOwner(gameObject);

            yield return new WaitForSeconds(rifleSpawnInterval);
        }
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
            ui.UpdateHealth((int)health, (int)stats.maxHealth);
            GameManager.instance.state = GameStates.GameOver;
            GameOverScreen.SetActive(true);
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