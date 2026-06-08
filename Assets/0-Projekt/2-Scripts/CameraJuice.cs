using UnityEngine;

public class CameraJuice : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform playerTransform; 

    [Header("Lean Settings (Drehung)")]
    [SerializeField] private float leanAmount = 0.05f;      
    [SerializeField] private float leanSpeed = 10f;       

    [Header("Shift Settings (Verschiebung)")]
    [SerializeField] private float shiftAmount = 0.09f;   
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
        // NEU: Wenn der GameManager im Pausenmodus ist (z.B. Inventar offen), 
        // brechen wir sofort ab und bewegen die Kamera nicht weiter!
        if (GameManager.instance != null)
        {
            if (GameManager.instance.state == GameStates.paused || 
                GameManager.instance.state == GameStates.GameOver)
            {
                // Wir merken uns trotzdem die aktuelle Position des Spielers, 
                // damit es beim Schließen des Inventars keinen riesigen Ruckler gibt
                if (playerTransform != null)
                {
                    lastPlayerPosition = playerTransform.position;
                }
                return; 
            }
        }

        if (playerTransform == null) return;

        // 1. Bewegung berechnen
        Vector3 playerMovement = playerTransform.position - lastPlayerPosition;
        Vector3 localMovement = transform.InverseTransformDirection(playerMovement);

        // 2. KAMERA-SCHWENK
        float targetLeanZ = -localMovement.x * leanAmount * 10f;
        targetLeanZ = Mathf.Clamp(targetLeanZ, -leanAmount, leanAmount);
        currentLeanZ = Mathf.Lerp(currentLeanZ, targetLeanZ, Time.deltaTime * leanSpeed);

        // 3. KAMERA-SHIFT
        Vector3 targetShift = playerMovement * shiftAmount * 10f;
        currentShift = Vector3.Lerp(currentShift, targetShift, Time.deltaTime * shiftSpeed);

        // 4. Werte anwenden
        Vector3 currentAngles = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(currentAngles.x, currentAngles.y, currentLeanZ);

        transform.position += currentShift;

        lastPlayerPosition = playerTransform.position;
    }
}