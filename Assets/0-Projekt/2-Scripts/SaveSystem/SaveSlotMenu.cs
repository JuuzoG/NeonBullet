using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// One panel with a button per slot. In the inspector, add one "SlotButton" entry
// per slot you want to offer (e.g. 3 entries for 3 save slots), each pointing at
// its own Button + TMP_Text in the UI.
public class SaveSlotMenu : MonoBehaviour
{
    [Serializable]
    public class SlotButton
    {
        public int slotIndex;
        public Button button;
        public TMP_Text label;
        public Button deleteButton;
    }

    public static SaveSlotMenu instance;

    public GameObject panel;
    public List<SlotButton> slotButtons = new List<SlotButton>();

    [Header("New Game")]
    [Tooltip("Shown for a few seconds when the player tries to start a new game but every slot is occupied.")]
    public GameObject noEmptySlotsMessage;
    public float noEmptySlotsMessageDuration = 2f;

    private Action<int> onSlotChosen;
    private bool onlyEmptySlots;

    void Awake()
    {
        instance = this;
        if (panel != null) panel.SetActive(false);

        foreach (SlotButton sb in slotButtons)
        {
            int slot = sb.slotIndex; // capture locally for the closure
            sb.button.onClick.AddListener(() => ChooseSlot(slot));

            if (sb.deleteButton != null)
                sb.deleteButton.onClick.AddListener(() => DeleteSlot(slot));
        }
    }

    // Call this with what should happen once the player picks a slot,
    // e.g. SaveSlotMenu.instance.Open(slot => SaveManager.instance.Save(slot));
    // Pass onlyEmptySlots = true to grey out slots that already have a save
    // (used for "New Game" so the player can't pick straight into an overwrite).
    public void Open(Action<int> onChosen, bool onlyEmptySlots = false)
    {
        onSlotChosen = onChosen;
        this.onlyEmptySlots = onlyEmptySlots;
        RefreshLabels();

        if (panel != null) panel.SetActive(true);
        if (GameManager.instance != null) GameManager.instance.state = GameStates.paused;
        Time.timeScale = 0;
    }

    // Opens the picker restricted to empty slots for starting a new game.
    // If every slot is occupied, shows noEmptySlotsMessage instead and tells
    // the player to delete a save first, rather than opening the menu at all.
    public void OpenForNewGame(Action<int> onChosen)
    {
        bool anyEmptySlot = false;
        foreach (SlotButton sb in slotButtons)
        {
            if (!SaveManager.instance.HasSave(sb.slotIndex))
            {
                anyEmptySlot = true;
                break;
            }
        }

        if (!anyEmptySlot)
        {
            ShowNoEmptySlotsMessage();
            return;
        }

        Open(onChosen, onlyEmptySlots: true);
    }

    private void ShowNoEmptySlotsMessage()
    {
        if (noEmptySlotsMessage == null)
        {
            Debug.LogWarning("SaveSlotMenu: All save slots are full. Delete a save to start a new game.");
            return;
        }

        StopCoroutine(nameof(ShowNoEmptySlotsMessageRoutine));
        StartCoroutine(nameof(ShowNoEmptySlotsMessageRoutine));
    }

    private System.Collections.IEnumerator ShowNoEmptySlotsMessageRoutine()
    {
        noEmptySlotsMessage.SetActive(true);
        yield return new WaitForSecondsRealtime(noEmptySlotsMessageDuration);
        noEmptySlotsMessage.SetActive(false);
    }

    public void Close()
    {
        if (panel != null) panel.SetActive(false);
        if (GameManager.instance != null) GameManager.instance.state = GameStates.inGame;
        Time.timeScale = 1;
    }

    private void RefreshLabels()
    {
        foreach (SlotButton sb in slotButtons)
        {
            bool hasSave = SaveManager.instance.HasSave(sb.slotIndex);
            SaveData data = hasSave ? SaveSystem.Load(sb.slotIndex) : null;

            if (sb.label != null)
            {
                sb.label.text = data != null
                    ? $"Slot {sb.slotIndex + 1}\n{data.savedAt}"
                    : $"Empty";
            }

            if (sb.button != null)
            {
                sb.button.interactable = !onlyEmptySlots || !hasSave;
            }
        }
    }

    // Deletes the save in this slot and immediately refreshes the labels/buttons
    // so it shows as "Empty" right away. Wire a per-slot delete button to this.
    public void DeleteSlot(int slot)
    {
        SaveManager.instance.DeleteSave(slot);
        RefreshLabels();
    }

    private void ChooseSlot(int slot)
    {
        if (onlyEmptySlots && SaveManager.instance.HasSave(slot))
            return; // shouldn't happen since the button is disabled, but guard anyway

        onSlotChosen?.Invoke(slot);
        onSlotChosen = null;
        Close();
    }
}