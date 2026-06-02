using UnityEngine;

public class CameraJuice : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform playerTransform; 

    [Header("Lean Settings (Drehung)")]
    // Ein kleinerer Wert (z.B. 0.5 oder 1) macht den Schwenk jetzt spürbar schwächer!
    [SerializeField] private float leanAmount = 0.1f;      
    [SerializeField] private float leanSpeed = 15f;       

    [Header("Shift Settings (Verschiebung)")]
    [SerializeField] private float shiftAmount = 0.2f;   
    [SerializeField] private float shiftSpeed = 4f;      

    private Vector3 lastPlayerPosition;
    private float currentLeanZ = 0f;
    private Vector3 currentShift = Vector3.zero;

    void Start()
    {
        if (playerTransform == null && GameManager.instance != null && GameManager.instance.player != null)
        {
            playerTransform = GameManager.instance.player.transform;
        }

        if (playerTransform != null)
        {
            lastPlayerPosition = playerTransform.position;
        }
    }

    void LateUpdate()
    {
        if (playerTransform == null) return;

        // 1. Bewegung berechnen
        Vector3 playerMovement = playerTransform.position - lastPlayerPosition;
        
        // Umrechnung in lokale Kamerarichtung
        Vector3 localMovement = transform.InverseTransformDirection(playerMovement);

        // 2. KAMERA-SCHWENK (Überarbeitete Berechnung)
        // Wir nutzen Time.deltaTime, damit die Geschwindigkeit des Spielers den Schwenk nicht extrem verzerrt
        float targetLeanZ = -localMovement.x * leanAmount * 10f;
        
        // Das Limit fängt den Wert jetzt sauber ab
        targetLeanZ = Mathf.Clamp(targetLeanZ, -leanAmount, leanAmount);
        
        // Sanftes Einschwenken
        currentLeanZ = Mathf.Lerp(currentLeanZ, targetLeanZ, Time.deltaTime * leanSpeed);

        // 3. KAMERA-SHIFT (Vorschauen)
        Vector3 targetShift = playerMovement * shiftAmount * 10f;
        currentShift = Vector3.Lerp(currentShift, targetShift, Time.deltaTime * shiftSpeed);

        // 4. Werte anwenden
        Vector3 currentAngles = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(currentAngles.x, currentAngles.y, currentLeanZ);

        transform.position += currentShift;

        lastPlayerPosition = playerTransform.position;
    }
}