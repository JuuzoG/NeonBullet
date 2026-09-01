using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider healthSlider;
    
    private Camera mainCamera;
    private Component targetEnemy; // Wir speichern den Gegner als allgemeine Komponente
    private bool isRanged;
    private bool enemyInitialized = false;

    void Start()
    {
        mainCamera = Camera.main;
        FindEnemy();
    }

    void FindEnemy()
    {
        // Wir suchen zuerst nach dem Melee-Enemy
        targetEnemy = GetComponentInParent<EnemyOverhaul>();
        if (targetEnemy != null)
        {
            isRanged = false;
            enemyInitialized = true;
            return;
        }

        // Wenn nicht gefunden, suchen wir nach dem Ranged-Enemy
        targetEnemy = GetComponentInParent<RangedEnemy>();
        if (targetEnemy != null)
        {
            isRanged = true;
            enemyInitialized = true;
        }
    }

    void Update()
    {
        // 1. Billboard-Effekt: Health Bar schaut immer zur Kamera
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
        }

        // 2. Lebensbalken aktualisieren
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        if (healthSlider == null) return;

        // Sicherheitscheck: Wenn der Gegner zerstört wurde, lösche sofort diese Health Bar!
        if (enemyInitialized && targetEnemy == null)
        {
            Destroy(gameObject);
            return;
        }

        // Falls beim Start noch kein Gegner da war, versuchen wir es nochmal
        if (!enemyInitialized)
        {
            FindEnemy();
            return;
        }

        try 
        {
            float currentHealth = 0;
            float maxHealth = 1;

            // Wir holen uns die Werte sicher über Reflection
            if (!isRanged)
            {
                var enemyScript = (EnemyOverhaul)targetEnemy;
                currentHealth = (float)enemyScript.GetType()
                    .GetField("health", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .GetValue(enemyScript);
                maxHealth = enemyScript.stats.maxHealth;
            }
            else
            {
                var rangedScript = (RangedEnemy)targetEnemy;
                currentHealth = (float)rangedScript.GetType()
                    .GetField("health", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .GetValue(rangedScript);
                maxHealth = rangedScript.stats.maxHealth;
            }

            // Slider aktualisieren
            healthSlider.value = currentHealth / maxHealth;

            // Falls der Gegner im selben Frame stirbt, blenden wir die Bar schon mal aus
            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        catch (System.Exception)
        {
            // Falls IRGENDEIN Fehler auftritt (z.B. weil der Gegner genau JETZT gelöscht wird),
            // zerstören wir die Health Bar einfach direkt, damit sie nicht hängen bleibt.
            Destroy(gameObject);
        }
    }
}