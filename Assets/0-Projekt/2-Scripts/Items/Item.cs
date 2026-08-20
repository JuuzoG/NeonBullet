using UnityEngine;

public class Item : MonoBehaviour
{
    private const float pickUpRange = 3f;
    public GameObject nameDisplay;
    public ItemData itemData;

    [Tooltip("Unique id for THIS specific item instance in the world. Right-click this component and choose 'Generate Id' once per object, then never change it.")]
    public string pickupId;

    void Start()
    {
        if (SaveManager.instance != null && SaveManager.instance.IsPickupCollected(pickupId))
        {
            Destroy(gameObject);
        }
    }

    void OnMouseOver()
    {
        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;

        Vector3 playerPosition = GameManager.instance.player.transform.position;
        float distance = (transform.position - playerPosition).magnitude;
        if (distance > pickUpRange) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameManager.instance.inventory.CollectItem(itemData);
            ItemNote.instance.Show(itemData.id);

            if (SaveManager.instance != null)
                SaveManager.instance.MarkPickupCollected(pickupId);

            Destroy(gameObject);
        }
    }

    void OnMouseEnter()
    {
        nameDisplay.SetActive(true);
    }

    void OnMouseExit()
    {
        nameDisplay.SetActive(false);
    }

    [ContextMenu("Generate Id")]
    private void GenerateId()
    {
        pickupId = System.Guid.NewGuid().ToString();
    }
}