using System;
using System.Collections.Generic;

[Serializable]
public class InventoryEntrySaveData
{
    public string id;
    public int amount;
}

[Serializable]
public class SaveData
{
    public string sceneName;

    public float posX, posY, posZ;
    public float rotY;

    public int munition;

    public List<InventoryEntrySaveData> items = new List<InventoryEntrySaveData>();

    // For display in a save-slot UI later (e.g. "Slot 1 - 19.08.2026 14:32")
    public string savedAt;
}
