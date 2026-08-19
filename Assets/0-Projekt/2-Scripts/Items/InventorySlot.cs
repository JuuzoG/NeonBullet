using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    public Image itemDisplay;
    private CollectedItem assignedItem;

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