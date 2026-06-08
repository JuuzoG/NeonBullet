using UnityEngine;
using UnityEngine.UI;

public class HubUI : MonoBehaviour
{
    [Header("Health")]
    public RectMask2D hpMask;
    public RectTransform hpBarRect;

    [Header("Energy")]
    public Image[] energyIcons;

    private float maxVisibleWidth;
    private float initialRightPadding;

    private void Start()
    {
        initialRightPadding = hpMask.padding.z;

        maxVisibleWidth = hpBarRect.rect.width
                        - hpMask.padding.x
                        - initialRightPadding;
    }

    public void UpdateHealth(int current, int max)
    {
        float percent = Mathf.Clamp01((float)current / max);

        Vector4 padding = hpMask.padding;

        // Full HP = initial padding
        // Empty HP = initial padding + maxVisibleWidth
        padding.z = initialRightPadding + (1f - percent) * maxVisibleWidth;

        hpMask.padding = padding;
    }

    public void UpdateEnergy(int current, int max)
    {
        current = Mathf.Clamp(current, 0, energyIcons.Length);

        for (int i = 0; i < energyIcons.Length; i++)
        {
            energyIcons[i].enabled = i < current;
        }
    }
}