using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Attach to the root RectTransform of your notice panel.
/// Call Show(itemName) from your pickup logic.
/// </summary>
public class ItemNote : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform noticePanel;
    [SerializeField] private TextMeshProUGUI itemNameText;

    [Header("Slide Settings")]
    [SerializeField] private float offscreenX    = 600f;  // X position when hidden (right, off-screen)
    [SerializeField] private float onscreenX     =  0f;   // X position when visible (resting position)
    [SerializeField] private float slideInTime   = 0.35f;
    [SerializeField] private float holdTime      = 1.8f;
    [SerializeField] private float slideOutTime  = 0.35f;

    [Header("Easing")]
    private AnimationCurve slideInCurve  = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private AnimationCurve slideOutCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Yes")]
    public static ItemNote instance;

    private Coroutine _activeRoutine;

    private void Awake()
    {
        instance = this;
        SetX(offscreenX);
    }

    public void Show(string itemName)
    {
        if (itemNameText != null) itemNameText.text = itemName;

        if (_activeRoutine != null) StopCoroutine(_activeRoutine);
        _activeRoutine = StartCoroutine(AnimateNotice());
    }

    private IEnumerator AnimateNotice()
    {
        // --- Slide IN (right → center) ---
        yield return Slide(offscreenX, onscreenX, slideInTime, slideInCurve);

        // --- Hold ---
        yield return new WaitForSeconds(holdTime);

        // --- Slide OUT (center → right) ---
        yield return Slide(onscreenX, offscreenX, slideOutTime, slideOutCurve);
    }

    private IEnumerator Slide(float fromX, float toX, float duration, AnimationCurve curve)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            SetX(Mathf.LerpUnclamped(fromX, toX, curve.Evaluate(t)));
            yield return null;
        }
        SetX(toX);
    }

    private void SetX(float x)
    {
        Vector2 pos = noticePanel.anchoredPosition;
        pos.x = x;
        noticePanel.anchoredPosition = pos;
    }
}
