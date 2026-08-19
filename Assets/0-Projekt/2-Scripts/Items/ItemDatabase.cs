using System.Collections.Generic;
using UnityEngine;

// Create one asset of this via: Assets > Create > Scriptable Objects > ItemDatabase
// Drag every ItemData asset you have into "allItems" so saves can be resolved back
// into real ItemData references by id.
[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Scriptable Objects/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> allItems = new List<ItemData>();

    private Dictionary<string, ItemData> lookup;

    public ItemData GetById(string id)
    {
        if (lookup == null)
        {
            lookup = new Dictionary<string, ItemData>();
            foreach (ItemData item in allItems)
            {
                if (item == null || string.IsNullOrEmpty(item.id)) continue;
                lookup[item.id] = item;
            }
        }

        lookup.TryGetValue(id, out ItemData result);
        return result;
    }
}
