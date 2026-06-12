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

    [Header("Animations")]
    [SerializeField] private Animator inventoryAnimator;
<<<<<<< Updated upstream
=======
    [SerializeField] private Animator panelAnimator;
>>>>>>> Stashed changes

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

        if (isVisible)
        {
            if (Input.GetKeyDown(player.Menu) || Input.GetKeyDown(player.Inventar))
            {
                ToggleInventoryFromKeyboard();
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
<<<<<<< Updated upstream
=======
            // SHOW UI BEFORE OPEN ANIMATION
>>>>>>> Stashed changes
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }

<<<<<<< Updated upstream
            SetSlotsVisible(false);

            if (inventoryAnimator != null)
            {
                inventoryAnimator.SetBool("isOpen", true);
            }
=======
            SetSlotsVisible(false); // HIDE SLOTS DURING OPEN ANIMATION

            inventoryAnimator.SetTrigger("Open"); // INVENTORY OPEN ANIMATION
            panelAnimator.SetTrigger("Open");     // PANEL OPEN ANIMATION
>>>>>>> Stashed changes

            GameManager.instance.state = GameStates.paused;
            Time.timeScale = 0;

<<<<<<< Updated upstream
            StartCoroutine(ShowSlotsAfterDelay());
        }
        else
        {
            if (inventoryAnimator != null)
            {
                inventoryAnimator.SetBool("isOpen", false);
            }

            StartCoroutine(HideInventoryAfterDelay());
=======
            StartCoroutine(ShowSlotsAfterDelay()); // DELAY SLOT DISPLAY
        }
        else
        {
            inventoryAnimator.SetTrigger("Close"); // INVENTORY CLOSE ANIMATION
            panelAnimator.SetTrigger("Close");     // PANEL CLOSE ANIMATION

            StartCoroutine(HideInventoryAfterDelay()); // WAIT FOR CLOSE ANIMATION
>>>>>>> Stashed changes

            GameManager.instance.state = GameStates.inGame;
            Time.timeScale = 1;
        }
    }

<<<<<<< Updated upstream
    private void SetSlotsVisible(bool visible)
    {
        foreach (InventorySlot slot in InventorySlots)
        {
            slot.gameObject.SetActive(visible);
        }
    }

=======
    // SLOT DISPLAY DELAY
>>>>>>> Stashed changes
    private IEnumerator ShowSlotsAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.25f);

        SetInventorySlots();
        SetSlotsVisible(true);
    }

<<<<<<< Updated upstream
=======
    // CLOSE ANIMATION DELAY
>>>>>>> Stashed changes
    private IEnumerator HideInventoryAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.25f);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

<<<<<<< Updated upstream
=======
    // SLOT VISIBILITY CONTROL
    private void SetSlotsVisible(bool visible)
    {
        foreach (InventorySlot slot in InventorySlots)
        {
            slot.gameObject.SetActive(visible);
        }
    }

>>>>>>> Stashed changes
    private IEnumerator FlashButtonColor()
    {
        if (inventoryButton == null)
            yield break;

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