using UnityEngine;

public class Item : MonoBehaviour
{
    private const float pickUpRange = 1.5f;
    public GameObject nameDisplay;
    public ItemData itemData;

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


}
