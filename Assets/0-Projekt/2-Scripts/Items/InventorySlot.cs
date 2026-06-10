using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (assignedItem == null) return;
        GameManager.instance.inventory.OnSlotHover(assignedItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.instance.inventory.OnSlotHoverExit();
    }
}