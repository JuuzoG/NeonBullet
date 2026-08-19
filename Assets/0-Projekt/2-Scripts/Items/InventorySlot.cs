using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image itemDisplay;
    public TMP_Text amount;
    private CollectedItem assignedItem;

    private void Start()
    {
        Corection();
    }

    private void Update()
    {
        Corection();
    }

    private void Corection()
    {
        if (itemDisplay.sprite == null)
        {
            amount.text = "";
            itemDisplay.color = Color.clear;
        }
        else
        {
            itemDisplay.color = Color.white;
            amount.text = ""+ assignedItem.amount;
        }
    }

    public void SetValues(CollectedItem item)
    {
        assignedItem = item;
        itemDisplay.sprite = (item == null) ? null : item.data.image;
        //amount.text = ""+ item.amount;
    }

    public void GetPressed()
    {
        GameManager.instance.inventory.SetSelectedItem(assignedItem);
    }


}