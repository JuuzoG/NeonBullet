using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public RectTransform healthBar;
    private float maxWidth;

    public Image[] energyIcons;


    private void Start()
    {
        maxWidth = healthBar.sizeDelta.x;
    }

    public void UpdateHealth(int current, int max)
    {
        float percent = (float)current / max;

        Vector2 size = healthBar.sizeDelta;
        size.x = maxWidth * percent;
        healthBar.sizeDelta = size;
    }

    public void UpdateEnergy(int current, int max)
    {
        for (int i = 0; i < energyIcons.Length; i++)
        {
            energyIcons[i].enabled = i < current;
        }
    }
}