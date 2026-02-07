using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootTable", menuName = "Scriptable Objects/LootTable")]
public class LootTable : ScriptableObject
{
    [SerializeField]
    private List<DropTable> dropTables;

    public GameObject GetDrop()
    {
        float random = Random.value;
        foreach (DropTable dropTable in dropTables)
        {
            if (random < dropTable.chance)
            {
                int id = Random.Range(0,dropTable.drops.Count);
                return dropTable.drops[id];
            }
            random -= dropTable.chance;
        }
        return null;
    }
}

[System.Serializable]
public struct DropTable
{
    public float chance;
    public List<GameObject> drops;
}