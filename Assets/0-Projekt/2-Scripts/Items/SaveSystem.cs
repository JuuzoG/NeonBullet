using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private const string FilePrefix = "save_slot_";
    private const string FileExtension = ".json";

    private static string GetPath(int slot)
    {
        return Path.Combine(Application.persistentDataPath, FilePrefix + slot + FileExtension);
    }

    public static void Save(int slot, SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(slot), json);
    }

    public static bool SaveExists(int slot)
    {
        return File.Exists(GetPath(slot));
    }

    public static SaveData Load(int slot)
    {
        string path = GetPath(slot);
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static void DeleteSave(int slot)
    {
        string path = GetPath(slot);
        if (File.Exists(path)) File.Delete(path);
    }
}