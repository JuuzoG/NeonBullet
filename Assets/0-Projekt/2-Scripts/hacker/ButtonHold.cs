using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonHold : MonoBehaviour
{
    [Header("Hold Settings")]
    [SerializeField] private float holdSec = 2f;
    public GameObject door;
    [SerializeField] private KeyCode holdAction = KeyCode.F;
    [SerializeField] private string playerTag = "Player";

    [Header("UI")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private GameObject progressBarRoot;

    private float _heldTime;
    private bool _inTrigger;

    private void Start()
    {
        if (progressBar != null)
            progressBar.value = 0f;

        if (progressBarRoot != null)
            progressBarRoot.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (string.IsNullOrEmpty(playerTag) || other.CompareTag(playerTag))
        {
            _inTrigger = true;

            if (progressBarRoot != null)
                progressBarRoot.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (string.IsNullOrEmpty(playerTag) || other.CompareTag(playerTag))
        {
            _inTrigger = false;
            _heldTime = 0f;

            if (progressBarRoot != null)
                progressBarRoot.SetActive(false);
        }
    }

    private void Update()
    {
        bool isHeld = _inTrigger && Input.GetKey(holdAction);

        if (isHeld)
        {
            _heldTime += Time.deltaTime;

            if (progressBar != null)
                progressBar.value = _heldTime / holdSec;

            if (_heldTime >= holdSec)
            {
                Trigger();
            }
        }
        else
        {
            _heldTime = 0f;

            if (progressBar != null)
                progressBar.value = 0f;
        }
    }

    private void Trigger()
    {
        Destroy(door);
        Destroy(gameObject);
    }
}