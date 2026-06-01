using UnityEngine;

public class CharMarker : MonoBehaviour
{
    [SerializeField] private float pulseSpeed = 1.8f;
    [SerializeField] private float pulseMinScale = 0.88f;
    [SerializeField] private float pulseMaxScale = 1.0f;

    [SerializeField] private float opacityMin = 0.55f;
    [SerializeField] private float opacityMax = 0.85f;

    [SerializeField] private bool spinEnabled = true;
    [SerializeField] private float spinSpeed = 18f; // degrees per second

    private Material _mat;
    private float _time;

    private void Awake()
    {
        var renderer = GetComponent<Renderer>();
        _mat = renderer.material; // creates instance
    }

    private void Update()
    {
        _time += Time.deltaTime;

        float t = (Mathf.Sin(_time * pulseSpeed * Mathf.PI) + 1f) * 0.5f;

        float scale = Mathf.Lerp(pulseMinScale, pulseMaxScale, t);
        transform.localScale = new Vector3(scale, 1f, scale);

        Color c = _mat.GetColor("_BaseColor");
        c.a = Mathf.Lerp(opacityMin, opacityMax, t);
        _mat.SetColor("_BaseColor", c);

        if (spinEnabled)
        {
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
        }
    }
}
