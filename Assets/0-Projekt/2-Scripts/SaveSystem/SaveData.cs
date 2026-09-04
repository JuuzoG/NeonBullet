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

    public bool hasRifle;
    public bool hasRailgun;
    public string equippedAbility; // AbilityType as string; "None" if nothing equipped

    public List<InventoryEntrySaveData> items = new List<InventoryEntrySaveData>();
    public List<string> collectedPickupIds = new List<string>();
    public string savedAt;
}