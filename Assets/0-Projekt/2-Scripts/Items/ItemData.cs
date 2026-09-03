using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public GameObject prefab;
    public Sprite image;
    public string id;
    public string description;
    public bool useable;

    public void Activate()
    {
        switch (id)
        {
            case "Epipen":
                GameManager.instance.player.GainHealth(25);
                break;
            case "Medkit":
                GameManager.instance.player.GainHealth(50);
                break;
        }
    }
}
