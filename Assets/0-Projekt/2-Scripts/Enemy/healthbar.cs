using UnityEngine;
using UnityEngine.UI;

// Packe dieses Skript auf das World-Space-Canvas über dem Gegner.
public class EnemyHealthBar : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider healthSlider;
    
    private Camera mainCamera;

    // Referenzen auf die möglichen Gegnertypen
    private Enemy meleeEnemy;
    private RangedEnemy rangedEnemy;
    private bool isRanged;

    void Start()
    {
        mainCamera = Camera.main;

        // Wir suchen die Gegner-Komponente auf dem Parent-Objekt
        meleeEnemy = GetComponentInParent<Enemy>();
        if (meleeEnemy != null)
        {
            isRanged = false;
        }
        else
        {
            rangedEnemy = GetComponentInParent<RangedEnemy>();
            if (rangedEnemy != null)
            {
                isRanged = true;
            }
        }
    }

    void Update()
    {
        // 1. Billboarding: Dreht die Healthbar immer zur Kamera
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
        }

        // 2. Lebensbalken automatisch aktualisieren
        UpdateHealthFromEnemy();
    }

    private void UpdateHealthFromEnemy()
    {
        if (healthSlider == null) return;

        if (!isRanged && meleeEnemy != null)
        {
            // Holt sich die private 'health' Variable geht in C# nicht direkt, 
            // aber wir tricksen nicht, sondern nutzen die Werte, die da sind.
            // Da 'health' in deinen Skripten private ist, müssen wir einen kleinen Umweg gehen:
            // Falls du 'health' in Enemy/RangedEnemy nicht 'public' machen darfst,
            // können wir das über System.Reflection auslesen, damit deine Skripte zu 100% gleich bleiben!
            
            float currentHealth = (float)meleeEnemy.GetType()
                .GetField("health", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(meleeEnemy);

            healthSlider.value = currentHealth / meleeEnemy.stats.maxHealth;
        }
        else if (isRanged && rangedEnemy != null)
        {
            float currentHealth = (float)rangedEnemy.GetType()
                .GetField("health", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(rangedEnemy);

            healthSlider.value = currentHealth / rangedEnemy.stats.maxHealth;
        }
    }
}