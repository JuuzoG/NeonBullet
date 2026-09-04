using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public ItemDatabase itemDatabase;

    [Header("New Game Defaults")]
    public string newGameSceneName;
    public Vector3 newGamePosition;
    public float newGameRotationY;
    public int newGameMunition = 10;
    public bool newGameHasRifle = true;
    public bool newGameHasRailgun = true;

    public string introSceneName;

    private SaveData pendingLoad;
    public int currentSlot { get; private set; } = -1;
    public bool HasCurrentSlot => currentSlot >= 0;


    private HashSet<string> collectedPickupIds = new HashSet<string>();

    [Header("Debug")]
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
            hasRifle = player.Rifle,
            hasRailgun = player.Railgun,
            equippedAbility = player.equippedAbility.ToString(),
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
        collectedPickupIds = new HashSet<string>(data.collectedPickupIds ?? new List<string>());
        SyncDebugList();

        currentSlot = slot;
        pendingLoad = data;

        if (SceneTransitionManager.instance != null)
        {
            SceneTransitionManager.instance.FadeAndLoad(data.sceneName, () => StartCoroutine(ApplyLoadedDataNextFrame()));
        }
        else
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(data.sceneName);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StartCoroutine(ApplyLoadedDataNextFrame());
    }

    private IEnumerator ApplyLoadedDataNextFrame()
    {
        yield return null;

        SaveData data = pendingLoad;
        pendingLoad = null;
        if (data == null) yield break;

        Player player = GameManager.instance.player;
        Inventory inventory = GameManager.instance.inventory;

        if (player != null)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.position = new Vector3(data.posX, data.posY, data.posZ);
                rb.rotation = Quaternion.Euler(0, data.rotY, 0);
            }
            else
            {
                player.transform.position = new Vector3(data.posX, data.posY, data.posZ);
                player.transform.rotation = Quaternion.Euler(0, data.rotY, 0);
            }

            player.munition = data.munition;
            player.Rifle = data.hasRifle;
            player.Railgun = data.hasRailgun;

            AbilityType savedAbility = AbilityType.None;
            if (!string.IsNullOrEmpty(data.equippedAbility))
                Enum.TryParse(data.equippedAbility, out savedAbility);
            player.EquipAbility(savedAbility);

            player.GainHealth(player.stats.maxHealth);
        }

        if (inventory != null && itemDatabase != null)
        {
            inventory.LoadItems(data.items, itemDatabase);
        }
    }

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
            hasRifle = newGameHasRifle,
            hasRailgun = newGameHasRailgun,
            equippedAbility = AbilityType.None.ToString(),
            savedAt = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
        };

        SaveSystem.Save(slot, data);
        currentSlot = slot;

        if (!string.IsNullOrEmpty(introSceneName) && SceneTransitionManager.instance != null)
        {
            SceneTransitionManager.instance.FadeAndLoad(introSceneName);
        }
        else
        {
            if (!string.IsNullOrEmpty(introSceneName) && SceneTransitionManager.instance == null)
                Debug.LogWarning("SaveManager: introSceneName is set but SceneTransitionManager.instance is null " +
                                  "(is a SceneTransitionManager present in the current scene?). Skipping intro.");

            Load(slot);
        }
    }

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

    public bool IsPickupCollected(string pickupId)
    {
        return !string.IsNullOrEmpty(pickupId) && collectedPickupIds.Contains(pickupId);
    }

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