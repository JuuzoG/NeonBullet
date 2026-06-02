using UnityEngine;
using System.Collections;

public class ProjectileColorFlasher : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material neonMaterial;
    [SerializeField] private Material grayMaterial;

    [Header("Timing Settings")]
    [SerializeField] private float normalDuration = 2f;
    [SerializeField] private float flashDuration = 0.3f;

    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null && neonMaterial != null && grayMaterial != null)
        {
            StartCoroutine(FlashRoutine());
        }
    }

    private IEnumerator FlashRoutine()
    {
        while (true)
        {
            // Neon phase
            meshRenderer.material = neonMaterial;
            yield return new WaitForSeconds(normalDuration);

            // Gray phase
            meshRenderer.material = grayMaterial;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}