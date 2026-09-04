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
    public KeyCode Inventar;
    public KeyCode Q;
    public KeyCode Menu;

    [Header("Additionals")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject muselVFX;
    [SerializeField] private HubUI ui;
    [SerializeField] private GameObject GameOverScreen;

    [Header("Weapons")]
    private Railgun railgun;

    [Header("Low Health Feedback")]
    public CanvasGroup lowHealthVignette;
    public float lowHealthThreshold = 35f;
    public float vignettePulseSpeed = 2f;
    [Range(0f, 1f)] public float vignetteMinAlpha = 0.15f;
    [Range(0f, 1f)] public float vignetteMaxAlpha = 0.55f;

    private bool lowHealthActive;

    [Header("Rifle Settings")]
    public int rifleShotCount = 5;
    public float rifleSpreadAngle = 5f;
    public float rifleSpawnInterval = 0.05f;
    [SerializeField] private Transform muzzlePoint;

    [Header("Unlocked")]
    public bool Rifle = true;
    public bool Railgun = true;

    [Header("Special Ability")]
    public AbilityType equippedAbility = AbilityType.None;

    private Vector3 position;
    private Vector3 muselpos;

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

        UpdateLowHealthVignette();
    }

    void Update()
    {
        position = new Vector3(transform.position.x,transform.position.y+1.5f,transform.position.z);
        muselpos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 1.5f);

        if (lowHealthActive && lowHealthVignette != null)
        {
            float t = (Mathf.Sin(Time.time * vignettePulseSpeed) + 1f) * 0.5f;
            lowHealthVignette.alpha = Mathf.Lerp(vignetteMinAlpha, vignetteMaxAlpha, t);
        }

        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;
        if (GameManager.instance.state == GameStates.inventory) return;

        if (Input.GetKeyDown(shot) && munition > 0)
        {
            switch (GameManager.instance.WeaponSelect.CurrentWeaponIndex)
            {
                case 0:
                    GameObject proj = Instantiate(projectilePrefab, muzzlePoint.position, muzzlePoint.rotation);
                    Projectile p = proj.GetComponent<Projectile>();
                    if (p != null) p.SetOwner(gameObject);
                    Instantiate(muselVFX, muzzlePoint.position, muzzlePoint.rotation * Quaternion.Euler(0, -90f, 0));
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

        for (int i = 0; i < shotsToFire; i++)
        {
            float angleOffset = (i - (shotsToFire - 1) / 2f) * rifleSpreadAngle;
            Quaternion rotation = transform.rotation * Quaternion.Euler(0, 0, angleOffset);

            GameObject proj = Instantiate(projectilePrefab, muzzlePoint.position, rotation);
            Projectile p = proj.GetComponent<Projectile>();
            if (p != null) p.SetOwner(gameObject);
            Instantiate(muselVFX, muzzlePoint.position, rotation * Quaternion.Euler(0, -90f, 0));

            yield return new WaitForSeconds(rifleSpawnInterval);
        }
    }

    public void GainMunition(int amount)
    {
        munition += amount;
    }

    public void EquipAbility(AbilityType ability)
    {
        equippedAbility = ability;

        SpecialAttack specialAttack = GameManager.instance.specialAttack;
        if (specialAttack == null) return;

        specialAttack.Dash = ability == AbilityType.Dash;
        specialAttack.Explosion = ability == AbilityType.Explosion;
        specialAttack.Gambeling = ability == AbilityType.Gambeling;
    }

    public void GainHealth(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, stats.maxHealth);

        if (ui != null)
        {
            ui.UpdateHealth((int)health, (int)stats.maxHealth);
        }

        UpdateLowHealthVignette();

        if (health <= 0)
        {
            ui.UpdateHealth((int)health, (int)stats.maxHealth);
            GameManager.instance.state = GameStates.GameOver;
            GameOverScreen.SetActive(true);
        }
    }

    private void UpdateLowHealthVignette()
    {
        if (lowHealthVignette == null) return;

        bool shouldBeActive = health > 0 && health <= lowHealthThreshold;
        if (shouldBeActive == lowHealthActive) return;

        lowHealthActive = shouldBeActive;
        lowHealthVignette.gameObject.SetActive(shouldBeActive);

        if (!shouldBeActive)
            lowHealthVignette.alpha = 0f;
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