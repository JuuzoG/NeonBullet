using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public InventorySlot[] InventorySlots;
    private List<CollectedItem> items = new List<CollectedItem>();
    private CollectedItem selectedItem;
    private bool isVisible;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameManager.instance.inventory = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state == GameStates.GameOver) return;
        
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape) || Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            isVisible = !isVisible;
            selectedItem = null;
            for (int i=0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(isVisible);
            }   
            if (isVisible)
            {
                GameManager.instance.state = GameStates.paused;
                Time.timeScale = 0;
                SetInventorySlots();
            }
            else
            {
                GameManager.instance.state = GameStates.inGame;
                Time.timeScale = 1;
            }
        }
    }

    private void SetInventorySlots()
    {
        for (int i=0; i < InventorySlots.Length; i++)
        {
            if (i >= items.Count)
            {
                InventorySlots[i].SetValues(null);
            }
            else
            {
                InventorySlots[i].SetValues(items[i]);
            }
        }
    }

    public void SetSelectedItem(CollectedItem selection)
    {
        selectedItem = selection;
    }

    public void CollectItem(ItemData newItem)
    {
        foreach (CollectedItem item in items)
        {
            if (item.id == newItem.id)
            {
                item.amount++;
                return;
            }
        }
        items.Add(new CollectedItem(newItem));
    }

    public void ItemInteraction(bool isDropped)
    {
        if (selectedItem == null) return;

        if (isDropped)
        {
            Vector3 position = GameManager.instance.player.transform.position;
            GameObject prefab = selectedItem.data.prefab;
            Instantiate(prefab, position, Quaternion.identity);
        }
        else
        {
            selectedItem.data.Activate();
        }

        selectedItem.amount--;
        if (selectedItem.amount == 0)
        {
            items.Remove(selectedItem);
            selectedItem = null;
            SetInventorySlots();
        }
    }

}

public class CollectedItem
{
    public ItemData data;
    public string id;
    public int amount;

    public CollectedItem(ItemData data)
    {
        this.data = data;
        id = data.id;
        amount = 1;
    }
}
