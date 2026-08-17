using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public InventorySlot[] InventorySlots;
    public Button inventoryButton;

    [Header("HoverUI")]
    [SerializeField] private GameObject hoverPanel;
    [SerializeField] private TextMeshProUGUI hoverText;
    [SerializeField] private TextMeshProUGUI hoverTextDescription;

    [Header("Color")]
    [SerializeField] private Color pressedColor = Color.yellow;
    [SerializeField] private Color normalColor = Color.white;

    private List<CollectedItem> items = new List<CollectedItem>();
    private CollectedItem selectedItem;
    private bool isVisible;
    private Player player;

    void Awake()
    {
        GameManager.instance.inventory = this;

        GameObject playerGet = GameObject.FindGameObjectWithTag("Player");
        player = playerGet.GetComponent<Player>();
    }

    void Start()
    {
        if (inventoryButton != null)
        {
            inventoryButton.image.color = normalColor;
        }

        hoverPanel.SetActive(false);
    }

    void Update()
    {
        if (GameManager.instance.state == GameStates.GameOver)
            return;
        if (GameManager.instance.state == GameStates.paused)
            return;

        if (isVisible)
        {
            if (Input.GetKeyDown(player.Menu) || Input.GetKeyDown(player.Inventar))
            {
                ToggleInventoryFromKeyboard();
                StartCoroutine(FlashButtonColor());
            }
        }
        else
        {
            if (Input.GetKeyDown(player.Inventar))
            {
                ToggleInventoryFromKeyboard();
                StartCoroutine(FlashButtonColor());
            }
        }
    }

    public void OnSlotHover(CollectedItem item)
    {
        hoverPanel.SetActive(true);
        hoverText.text = item.data.id;
        hoverTextDescription.text = item.data.description;
    }

    public void OnSlotHoverExit()
    {
        hoverPanel.SetActive(false);
    }

    public void ToggleInventory()
    {
        ToggleInventoryInternal();
    }

    private void ToggleInventoryFromKeyboard()
    {
        Debug.Log("ToggleInventoryFromKeyboard called");

        ToggleInventoryInternal();

        if (inventoryButton != null)
        {
            EventSystem.current.SetSelectedGameObject(inventoryButton.gameObject);
        }
    }

    private void ToggleInventoryInternal()
    {
        isVisible = !isVisible;

        Debug.Log("Inventory Visible: " + isVisible);

        selectedItem = null;

        if (isVisible)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }

            SetSlotsVisible(false);

            GameManager.instance.state = GameStates.inventory;
            Time.timeScale = 0;

            StartCoroutine(ShowSlotsAfterDelay());
        }
        else
        {
            StartCoroutine(HideInventoryAfterDelay());

            GameManager.instance.state = GameStates.inGame;
            Time.timeScale = 1;
        }
    }

    private void SetSlotsVisible(bool visible)
    {
        foreach (InventorySlot slot in InventorySlots)
        {
            slot.gameObject.SetActive(visible);
        }
    }

    private IEnumerator ShowSlotsAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.25f);

        SetInventorySlots();
        SetSlotsVisible(true);
    }

    private IEnumerator HideInventoryAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.25f);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private IEnumerator FlashButtonColor()
    {
        if (inventoryButton == null) yield break;

        inventoryButton.image.color = pressedColor;
        yield return new WaitForSecondsRealtime(0.15f);
        inventoryButton.image.color = normalColor;
    }

    private void SetInventorySlots()
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (i >= items.Count)
                InventorySlots[i].SetValues(null);
            else
                InventorySlots[i].SetValues(items[i]);
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
        if (selectedItem == null)
            return;

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