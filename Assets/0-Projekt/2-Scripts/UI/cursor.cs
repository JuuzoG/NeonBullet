using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class cursor : MonoBehaviour
{
    [SerializeField]
    private InputActionReference pointerPositionAction;
    private RectTransform _cursorTransform;
    private Canvas _parentCanvas;
    private RectTransform _canvasRectTransform;
    private Camera _canvasCamera;
    public TMP_Text textField;
    public Image cursorImage;
    public Player player;
    private Enemy emy;

    private Color mouseColor;

    void Start()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        emy = enemy.GetComponent<Enemy>();
    }

    private void Awake()
    {
        _cursorTransform = GetComponent<RectTransform>();
        _parentCanvas = GetComponentInParent<Canvas>();
        if (_parentCanvas != null)
        {
            _canvasRectTransform = _parentCanvas.GetComponent<RectTransform>();
            _canvasCamera = _parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : _parentCanvas.worldCamera;
        }
    }

    private void OnEnable()
    {
        UnityEngine.Cursor.visible = false;
        pointerPositionAction.action.performed += OnPointerPositionChanged;
    }

    private void OnDisable()
    {
        UnityEngine.Cursor.visible = true;
        pointerPositionAction.action.performed -= OnPointerPositionChanged;
    }

    private void OnPointerPositionChanged(InputAction.CallbackContext ctx)
    {
        if (_cursorTransform == null || _canvasRectTransform == null) return;

        var mousePosition = ctx.ReadValue<Vector2>();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRectTransform,mousePosition, _canvasCamera,
             out var localPoint))
        {
            _cursorTransform.anchoredPosition = localPoint;
        }
    }

    private void Update()
    {
        textField.text = "" + player.munition;

        if (emy.onEnemy) mouseColor = new Color(1, 0, 0, 1);
        else if (player.munition == 0) {textField.color = new Color(1, 0, 0, 1); mouseColor = new Color(1, 1, 1, 0.5f);}
        else if (player.munition <= 5) {textField.color = new Color(1, 0.92f, 0.016f, 1); mouseColor = new Color(1, 1, 1, 1);}
        else {textField.color = new Color(1, 1, 1, 1); mouseColor = new Color(1, 1, 1, 1);}
        cursorImage.color = mouseColor;
    }
}
