using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    private const float pickUpRange = 3f;
    public GameObject nameDisplay;
    public ItemData itemData;
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
        if (GameManager.instance.state == GameStates.hacking) return;

        Vector3 playerPosition = GameManager.instance.player.transform.position;
        float distance = (transform.position - playerPosition).magnitude;
        if (distance > pickUpRange) return;
        if (Input.GetKeyDown(KeyCode.F))
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