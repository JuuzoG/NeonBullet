using UnityEngine;

// Put this on a GameObject with a trigger Collider in your scene.
// Make sure your Player object/root has the "Player" tag (Inventory.cs already relies on this).
public class SavePoint : MonoBehaviour
{
    [Tooltip("Key to press while inside the trigger to save.")]
    public KeyCode interactKey = KeyCode.F;

    [Tooltip("Optional UI prompt, e.g. 'Press F to save', shown while the player is in range.")]
    public GameObject savePrompt;

    [Tooltip("Optional feedback shown briefly after saving.")]
    public GameObject saveConfirmation;
    public float confirmationDuration = 1.5f;

    private bool playerInRange;

    void Start()
    {
        if (savePrompt != null) savePrompt.SetActive(false);
        if (saveConfirmation != null) saveConfirmation.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange) return;
        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;

        if (Input.GetKeyDown(interactKey))
        {
            TriggerSave();
        }
    }

    private void TriggerSave()
    {
        SaveSlotMenu.instance.Open(slot =>
        {
            SaveManager.instance.Save(slot);

            if (saveConfirmation != null)
            {
                StopAllCoroutines();
                StartCoroutine(ShowConfirmation());
            }
        });
    }

    private System.Collections.IEnumerator ShowConfirmation()
    {
        saveConfirmation.SetActive(true);
        yield return new WaitForSeconds(confirmationDuration);
        saveConfirmation.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        if (savePrompt != null) savePrompt.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        if (savePrompt != null) savePrompt.SetActive(false);
    }
}