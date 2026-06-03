using UnityEngine;

public class HubUI : MonoBehaviour
{
    public RectTransform healthBar;
    public RectTransform energyBar;

    public void UpdateHealth(int current, int max)
    {
        float percent = (float)current / max;
        healthBar.localScale = new Vector3(percent, 1f, 1f);
    }

    public void UpdateEnergy(int current, int max)
    {
        float percent = (float)current / max;
        energyBar.localScale = new Vector3(percent, 1f, 1f);
    }
}