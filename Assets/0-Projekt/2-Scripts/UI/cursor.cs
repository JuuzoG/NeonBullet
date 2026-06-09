using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class cursor : MonoBehaviour
{
    [SerializeField] private InputActionReference pointerPositionAction;
    [SerializeField] private InputActionReference gamepadAimAction;
    [SerializeField] private float gamepadAimRadius = 5f;

    private RectTransform _cursorTransform;
    private Canvas _parentCanvas;
    private RectTransform _canvasRectTransform;
    private Camera _canvasCamera;

    public TMP_Text textField;
    public GameObject cursorObj;
    private Image cursorImage;
    private Player player;
    private Animation cursorAnim;
    private Color mouseColor;

    private Vector2 _gamepadAimInput;
    private bool _usingGamepad = false;

    public static Vector3 WorldAimPosition { get; private set; }

    private void Start()
    {
        GameObject playerGet = GameObject.FindGameObjectWithTag("Player");
        player = playerGet.GetComponent<Player>();
        cursorImage = cursorObj.GetComponent<Image>();
        cursorAnim = cursorObj.GetComponent<Animation>();
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

        if (gamepadAimAction != null)
        {
            gamepadAimAction.action.performed += OnGamepadAim;
            gamepadAimAction.action.canceled += OnGamepadAim;
        }
    }

    private void OnDisable()
    {
        UnityEngine.Cursor.visible = true;
        pointerPositionAction.action.performed -= OnPointerPositionChanged;

        if (gamepadAimAction != null)
        {
            gamepadAimAction.action.performed -= OnGamepadAim;
            gamepadAimAction.action.canceled -= OnGamepadAim;
        }
    }

    private void OnPointerPositionChanged(InputAction.CallbackContext ctx)
    {
        if (_cursorTransform == null || _canvasRectTransform == null) return;

        _usingGamepad = false;

        var mousePosition = ctx.ReadValue<Vector2>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRectTransform, mousePosition, _canvasCamera, out var localPoint))
        {
            _cursorTransform.anchoredPosition = localPoint;
        }

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
            WorldAimPosition = hit.point;
    }

    private void OnGamepadAim(InputAction.CallbackContext ctx)
    {
        _gamepadAimInput = ctx.ReadValue<Vector2>();
        _usingGamepad = _gamepadAimInput.sqrMagnitude > 0.01f;
    }

    private void Update()
    {
        // Gamepad cursor + aim
        if (_usingGamepad && _gamepadAimInput.sqrMagnitude > 0.01f)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 aimDir = new Vector3(_gamepadAimInput.x, 0f, _gamepadAimInput.y);
            WorldAimPosition = playerPos + aimDir.normalized * gamepadAimRadius;

            Vector2 screenPoint = Camera.main.WorldToScreenPoint(WorldAimPosition);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRectTransform, screenPoint, _canvasCamera, out var localPoint))
            {
                _cursorTransform.anchoredPosition = localPoint;
            }
        }

        // Ammo display
        textField.text = "" + player.munition;

        if (Gamepad.current.rightTrigger.wasPressedThisFrame)
        {
            cursorAnim.CursorAnim();
        }
        else
        {
            if (player.munition == 0)       { textField.color = new Color(1, 0, 0, 1);            mouseColor = new Color(1, 1, 1, 0.3f); }
            else if (player.munition <= 5)  { textField.color = new Color(1, 0.92f, 0.016f, 1);   mouseColor = new Color(1, 1, 1, 1); }
            else                            { textField.color = new Color(1, 1, 1, 1);             mouseColor = new Color(1, 1, 1, 1); }
            cursorImage.color = mouseColor;
        }
    }
}