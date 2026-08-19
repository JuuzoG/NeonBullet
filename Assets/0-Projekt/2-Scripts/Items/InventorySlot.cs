using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    public Image itemDisplay;
    private CollectedItem assignedItem;

    private void Start()
    {
        ColorCorection();
    }

    private void Update()
    {
        ColorCorection();
    }

    private void ColorCorection()
    {
        if (itemDisplay.sprite == null) itemDisplay.color = Color.clear;
        else itemDisplay.color = Color.white;
    }

    public void SetValues(CollectedItem item)
    {
        assignedItem = item;
        itemDisplay.sprite = (item == null) ? null : item.data.image;
    }

    public void GetPressed()
    {
        GameManager.instance.inventory.SetSelectedItem(assignedItem);
    }


}