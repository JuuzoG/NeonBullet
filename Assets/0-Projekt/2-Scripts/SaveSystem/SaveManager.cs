using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Add this to a persistent object in your first/boot scene, same pattern as GameManager.
// Assign the ItemDatabase asset in the inspector.
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [Tooltip("Assign the ItemDatabase asset so saved item ids can be resolved back into ItemData.")]
    public ItemDatabase itemDatabase;

    [Header("New Game Defaults")]
    [Tooltip("Scene a brand new save should start in.")]
    public string newGameSceneName;
    public Vector3 newGamePosition;
    public float newGameRotationY;
    public int newGameMunition = 10;

    private SaveData pendingLoad;

    // Which slot is currently being played. -1 means no slot is active yet
    // (e.g. player hasn't saved or loaded anything this session).
    public int currentSlot { get; private set; } = -1;
    public bool HasCurrentSlot => currentSlot >= 0;

    // Ids of WorldPickups collected during the current playthrough. Written into
    // SaveData at Save() time, restored at Load() time, cleared on StartNewGame().
    private HashSet<string> collectedPickupIds = new HashSet<string>();

    [Header("Debug")]
    [Tooltip("Read-only mirror of collectedPickupIds, visible here in Play mode for debugging. Don't edit this directly, it's overwritten automatically.")]
    [SerializeField] private List<string> debugCollectedPickupIds = new List<string>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save(int slot)
    {
        Player player = GameManager.instance.player;
        Inventory inventory = GameManager.instance.inventory;

        if (player == null)
        {
            Debug.LogWarning("SaveManager: No player found, cannot save.");
            return;
        }

        // Heal to full on save, as requested.
        player.GainHealth(player.stats.maxHealth);

        SaveData data = new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name,
            posX = player.transform.position.x,
            posY = player.transform.position.y,
            posZ = player.transform.position.z,
            rotY = player.transform.eulerAngles.y,
            munition = player.munition,
            savedAt = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
        };

        if (inventory != null)
        {
            foreach (CollectedItem item in inventory.GetItems())
            {
                data.items.Add(new InventoryEntrySaveData
                {
                    id = item.id,
                    amount = item.amount
                });
            }
        }

        data.collectedPickupIds = new List<string>(collectedPickupIds);

        SaveSystem.Save(slot, data);
        currentSlot = slot;
        Debug.Log($"Game saved to slot {slot}.");
    }

    public void Load(int slot)
    {
        SaveData data = SaveSystem.Load(slot);
        if (data == null)
        {
            Debug.LogWarning($"SaveManager: No save found in slot {slot}.");
            return;
        }

        // Restore this BEFORE the scene loads. SceneManager.LoadScene runs Awake/Start
        // for every object in the new scene synchronously, before this method even
        // returns - so if we wait until the post-load coroutine to restore this,
        // every pickup's Start() check runs against stale data and never stays gone.
        collectedPickupIds = new HashSet<string>(data.collectedPickupIds ?? new List<string>());
        SyncDebugList();

        currentSlot = slot;
        pendingLoad = data;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(data.sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StartCoroutine(ApplyLoadedDataNextFrame());
    }

    private IEnumerator ApplyLoadedDataNextFrame()
    {
        // Wait a frame so Player/Inventory finish their own Awake/Start first
        // (Player.Awake sets GameManager.instance.player, etc.)
        yield return null;

        SaveData data = pendingLoad;
        pendingLoad = null;
        if (data == null) yield break;

        Player player = GameManager.instance.player;
        Inventory inventory = GameManager.instance.inventory;

        if (player != null)
        {
            player.transform.position = new Vector3(data.posX, data.posY, data.posZ);
            player.transform.rotation = Quaternion.Euler(0, data.rotY, 0);

            player.munition = data.munition;
            player.GainHealth(player.stats.maxHealth);
        }

        if (inventory != null && itemDatabase != null)
        {
            inventory.LoadItems(data.items, itemDatabase);
        }
    }

    // Writes a brand-new save (default scene/position/ammo, empty inventory) into
    // "slot" and immediately loads into it. Caller is responsible for making sure
    // "slot" is actually empty - use TryFindEmptySlot / HasSave to check first.
    public void StartNewGame(int slot)
    {
        collectedPickupIds = new HashSet<string>();
        SyncDebugList();

        SaveData data = new SaveData
        {
            sceneName = newGameSceneName,
            posX = newGamePosition.x,
            posY = newGamePosition.y,
            posZ = newGamePosition.z,
            rotY = newGameRotationY,
            munition = newGameMunition,
            savedAt = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
        };

        SaveSystem.Save(slot, data);
        Load(slot);
    }

    // Checks "slots" in order and returns the first one without an existing save.
    // Returns false (with emptySlot = -1) if every slot in the list is occupied.
    public bool TryFindEmptySlot(int[] slots, out int emptySlot)
    {
        foreach (int slot in slots)
        {
            if (!HasSave(slot))
            {
                emptySlot = slot;
                return true;
            }
        }

        emptySlot = -1;
        return false;
    }

    public bool HasSave(int slot) => SaveSystem.SaveExists(slot);
    public void DeleteSave(int slot) => SaveSystem.DeleteSave(slot);

    // Call from a pickup's Start() to know if it should hide itself.
    public bool IsPickupCollected(string pickupId)
    {
        return !string.IsNullOrEmpty(pickupId) && collectedPickupIds.Contains(pickupId);
    }

    // Call the moment a pickup is actually collected.
    public void MarkPickupCollected(string pickupId)
    {
        if (string.IsNullOrEmpty(pickupId)) return;
        collectedPickupIds.Add(pickupId);
        SyncDebugList();
    }

    private void SyncDebugList()
    {
        debugCollectedPickupIds = new List<string>(collectedPickupIds);
    }

    [ContextMenu("Log Collected Pickups")]
    private void LogCollectedPickups()
    {
        Debug.Log($"Collected pickups ({collectedPickupIds.Count}):\n" + string.Join("\n", collectedPickupIds));
    }
}