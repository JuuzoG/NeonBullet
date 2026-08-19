using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public ItemDatabase itemDatabase;
    private SaveData pendingLoad;

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

       
        player.GainHealth(player.stats.maxHealth); // Heal to full on save

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

        SaveSystem.Save(slot, data);
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

    public bool HasSave(int slot) => SaveSystem.SaveExists(slot);
    public void DeleteSave(int slot) => SaveSystem.DeleteSave(slot);
}
