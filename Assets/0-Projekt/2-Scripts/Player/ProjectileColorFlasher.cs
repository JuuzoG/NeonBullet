using UnityEngine;
using System.Collections;

public class ProjectileColorFlasher : MonoBehaviour
{
    [Header("Color Settings")]
    [SerializeField] private Color neonPinkColor = new Color(2f, 0f, 1.5f, 1f); // Knalliges Neon (HDR-Werte)
    [SerializeField] private Color grayColor = new Color(0.2f, 0.2f, 0.2f, 1f);     // Mattes Grau

    [Header("Timing Settings")]
    [SerializeField] private float normalDuration = 2f; 
    [SerializeField] private float flashDuration = 0.3f; 

    private MeshRenderer meshRenderer;
    private Material projectileMaterial;

    void Start()
    {
        // Holt sich den 3D Mesh Renderer, den man auf deinem Screenshot sieht
        meshRenderer = GetComponent<MeshRenderer>();
        
        if (meshRenderer != null)
        {
            // Erstellt eine eigene Material-Kopie für dieses Projektil
            projectileMaterial = meshRenderer.material;
            
            // Schaltet das Leuchten (Emission) im Shader frei
            projectileMaterial.EnableKeyword("_EMISSION");
            
            // Startet den unendlichen Rhythmus
            StartCoroutine(FlashRoutine());
        }
    }

    private IEnumerator FlashRoutine()
    {
        while (true)
        {
            // 1. Auf Neon-Pink schalten
            SetColor(neonPinkColor);
            yield return new WaitForSeconds(normalDuration);

            // 2. Auf Grau schalten
            SetColor(grayColor);
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void SetColor(Color targetColor)
    {
        if (projectileMaterial != null)
        {
            // Ändert die Grundfarbe
            projectileMaterial.SetColor("_Color", targetColor);
            // Ändert das Leuchten
            projectileMaterial.SetColor("_EmissionColor", targetColor);
            
            // Zwingt Unity, die Lichtberechnung sofort im Spiel zu aktualisieren
            DynamicGI.SetEmissive(meshRenderer, targetColor);
        }
    }

    private void OnDestroy()
    {
        // Löscht die Material-Kopie aus dem Speicher, wenn die Kugel verschwindet
        if (projectileMaterial != null)
        {
            Destroy(projectileMaterial);
        }
    }
}