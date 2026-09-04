using System.Collections;
using UnityEngine;

public class TimeStopAbility : MonoBehaviour
{
    [Header("Einstellungen")]
    [Tooltip("Taste zum Aktivieren der Fähigkeit")]
    public KeyCode activationKey = KeyCode.Space;

    [Tooltip("Wie lange die Zeit gestoppt bleibt (in Sekunden)")]
    public float duration = 3f;

    [Tooltip("Abklingzeit, bevor die Fähigkeit erneut genutzt werden kann")]
    public float cooldown = 5f;

    [Header("Spieler-Bewegung")]
    [Tooltip("Geschwindigkeit des Spielers während des Zeitstopps")]
    public float speed = 5f;

    private bool isTimeStopped = false;
    private bool isOnCooldown = false;
    private Rigidbody2D rb;

    void Start()
    {
        // Holt sich den Rigidbody2D vom Spieler
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Eingabe prüfen
        if (Input.GetKeyDown(activationKey) && !isTimeStopped && !isOnCooldown)
        {
            StartCoroutine(StopTimeRoutine());
        }

        // Eigenständige Steuerung des Spielers
       // HandleMovement();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;

        // Nutzt Time.unscaledDeltaTime, damit die Bewegung unabhängig von Time.timeScale funktioniert
        if (rb != null)
        {
            rb.MovePosition(rb.position + moveDirection * speed * Time.unscaledDeltaTime);
        }
        else
        {
            transform.Translate(moveDirection * speed * Time.unscaledDeltaTime);
        }
    }

    IEnumerator StopTimeRoutine()
    {
        isTimeStopped = true;

        // Hält die Zeit für die Spielwelt an
        Time.timeScale = 0f;

        // Wartet 3 Sekunden in realer Zeit (unscaled)
        yield return new WaitForSecondsRealtime(duration);

        // Setzt die Zeit wieder auf normal
        Time.timeScale = 1f;
        isTimeStopped = false;

        // Abklingzeit starten
        isOnCooldown = true;
        yield return new WaitForSecondsRealtime(cooldown);
        isOnCooldown = false;
    }

    void OnDisable()
    {
        // Stellt sicher, dass die Zeit wieder normal läuft, falls das Objekt deaktiviert wird
        Time.timeScale = 1f;
    }
}