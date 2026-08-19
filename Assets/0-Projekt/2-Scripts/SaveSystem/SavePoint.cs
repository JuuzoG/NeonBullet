using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public int saveSlot = 0; //Which save slot this save point writes to.
    public KeyCode interactKey = KeyCode.F;

    public GameObject savePrompt;

    public GameObject saveConfirmation;
    public float confirmationDuration = 1.5f;

    private bool playerInRange;

    void Start()
    {
        if (savePrompt != null) savePrompt.SetActive(false);
        if (saveConfirmation != null) saveConfirmation.SetActive(false);
    }

    void LateUpdate()
    {
        savePrompt.transform.parent.rotation = Camera.main.transform.rotation;
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
        SaveManager.instance.Save(saveSlot);

        if (saveConfirmation != null)
        {
            StopAllCoroutines();
            StartCoroutine(ShowConfirmation());
        }
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
