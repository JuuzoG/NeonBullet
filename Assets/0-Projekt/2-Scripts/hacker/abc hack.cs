using UnityEngine;
using System.Text;
using TMPro;

public class abchack : MonoBehaviour
{
    //Allmost all of this is writen by AI but I do understand what is happening
    [Header("Sequence Settings")]
    [SerializeField] private int letterAmount = 5;
    [SerializeField] private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    [SerializeField] private GameObject door;

    [Header("Interaction")]
    [SerializeField] private KeyCode Action = KeyCode.F;
    [SerializeField] private string playerTag = "Player";

    [Header("UI")]
    [SerializeField] private GameObject promptUI;     //"Press F" prompt
    [SerializeField] private GameObject sequenceUI;   // container shown while typing is active
    [SerializeField] private TextMeshProUGUI sequenceText;

    public System.Action OnActivated;

    private string targetSequence;
    private StringBuilder typedSequence = new StringBuilder();
    private bool _inTrigger;
    private bool _isTyping;

    private void Start()
    {
        GenerateNewSequence();
        UpdateUI();

        if (promptUI != null) promptUI.SetActive(false);
        if (sequenceUI != null) sequenceUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (string.IsNullOrEmpty(playerTag) || other.CompareTag(playerTag))
        {
            _inTrigger = true;
            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (string.IsNullOrEmpty(playerTag) || other.CompareTag(playerTag))
        {
            _inTrigger = false;
            _isTyping =false;

            if (promptUI != null) promptUI.SetActive(false);
            if (sequenceUI != null) sequenceUI.SetActive(false);
            GameManager.instance.state = GameStates.inGame;
        }
    }

    private void Update()
    {
        if (!_inTrigger) return;

        if (!_isTyping)
        {
            if (Input.GetKeyDown(Action))
            {
                StartTyping();
            }
            return;
        }

        // Currently typing -> read letter input
        if (string.IsNullOrEmpty(targetSequence)) return;

        foreach (char c in Input.inputString)
        {
            char upperChar = char.ToUpper(c);

            // Only accept letters
            if (upperChar < 'A' || upperChar > 'Z') continue;

            int currentIndex = typedSequence.Length;
            if (currentIndex >= targetSequence.Length) break;

            if (upperChar == targetSequence[currentIndex])
            {
                typedSequence.Append(upperChar);
                UpdateUI();

                if (typedSequence.Length == targetSequence.Length)
                {
                    ActivateVoid();
                }
            }
            else
            {
                // Wrong letter -> reset progress
                typedSequence.Clear();
                UpdateUI();
            }
        }
    }

    private void StartTyping()
    {
        _isTyping = true;
        GameManager.instance.state = GameStates.hacking;
        typedSequence.Clear();

        if (promptUI != null) promptUI.SetActive(false);
        if (sequenceUI != null) sequenceUI.SetActive(true);

        UpdateUI();
    }

    private void GenerateNewSequence()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < letterAmount; i++)
        {
            char letter = alphabet[Random.Range(0, alphabet.Length)];
            sb.Append(letter);
        }
        targetSequence = sb.ToString();
        typedSequence.Clear();
    }

    private void UpdateUI()
    {
        if (sequenceText == null || string.IsNullOrEmpty(targetSequence)) return;

        // Highlight typed letters in green, rest in white
        StringBuilder display = new StringBuilder();
        for (int i = 0; i < targetSequence.Length; i++)
        {
            if (i < typedSequence.Length)
                display.Append($"<color=green>{targetSequence[i]}</color>");
            else
                display.Append(targetSequence[i]);
        }

        sequenceText.text = display.ToString();
    }

    private void ActivateVoid()
    {
        GameManager.instance.state = GameStates.inGame;
        if (sequenceUI != null) sequenceUI.SetActive(false);
        Destroy(door);
        OnActivated?.Invoke();
        Destroy(gameObject);
    }

    void LateUpdate()
    {
        promptUI.transform.parent.rotation = Camera.main.transform.rotation;
    }

    public string GetTargetSequence() => targetSequence;
    public string GetTypedProgress() => typedSequence.ToString();
}