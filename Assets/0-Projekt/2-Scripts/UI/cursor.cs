using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
//von AI | bedeute das AI es erklärte hat.
public class Cursor : MonoBehaviour
{
    [SerializeField]
    private InputActionReference pointerPositionAction; //InputSystem UI/Point
    private RectTransform _cursorTransform; // die position des Cursor Object
    private Canvas _parentCanvas; // Canvas, in dem sich der Cursor befindet
    private RectTransform _canvasRectTransform; // RectTransform des Canvas, für Koordinatenumrechnung | von AI
    private Camera _canvasCamera; // Die Kamera
    public TMP_Text textField; //Amo count
    public GameObject cursorObj; //Object with Animation.cs, Image(Cursor) and Animator
    private Image cursorImage;
    private Player player;
    private Animation cursorAnim;
    private Color mouseColor;

    private void Start()
    {
        //alles nötige wird gefunden
        GameObject playerGEt = GameObject.FindGameObjectWithTag("Player");
        player = playerGEt.GetComponent<Player>();
        cursorImage = cursorObj.GetComponent<Image>();
        cursorAnim = cursorObj.GetComponent<Animation>();
    }

    private void Awake()
    {
        _cursorTransform = GetComponent<RectTransform>(); // Cursor RectTransform speichern
        _parentCanvas = GetComponentInParent<Canvas>(); // findet den Canvas des Cursor
        if (_parentCanvas != null)
        {
            _canvasRectTransform = _parentCanvas.GetComponent<RectTransform>();// RectTransform des Canvas speichern
            _canvasCamera = _parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _parentCanvas.worldCamera; // bei Overlay-Canvas wird keine Kamera benötigt, sonst die Canvas-Kamera verwenden | von AI
        }
    }

    private void OnEnable()
    {
        UnityEngine.Cursor.visible = false; // macht die normale mause unsichbar
        pointerPositionAction.action.performed += OnPointerPositionChanged; // Event abonnieren, wenn sich die Zeigerposition ändert | von AI
    }

    private void OnDisable()
    {
        UnityEngine.Cursor.visible = true; // macht die normale mause sichbar
        pointerPositionAction.action.performed -= OnPointerPositionChanged; // Event wieder abmelden, um Memory Leaks zu vermeiden | von AI
    }

    private void OnPointerPositionChanged(InputAction.CallbackContext ctx) // wird aufgerufen, sobald sich die Position des Input(UI/Point) ändert
    {
        if (_cursorTransform == null || _canvasRectTransform == null) return;

        var mousePosition = ctx.ReadValue<Vector2>();// aktuelle Bildschirmposition der Maus/des Pointers auslesen | von AI
        
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform,mousePosition, _canvasCamera,out var localPoint)) // Bildschirmkoordinate in lokale Canvas-Koordinate umrechnen | von AI
        {
            _cursorTransform.anchoredPosition = localPoint; // _cursorTransform an neue Position setzen
        }
    }

    private void Update()
    {
        if (GameManager.instance.state == GameStates.inGame) // Munitionsanzeige nur während des Spiels anzeigen, sonst leer lassen
        textField.text = "" + player.munition;
        else
        textField.text = "";

        if (Input.GetKeyDown(player.shot)) //Schuss-Animation abspielen
        {
            cursorAnim.CursorAnim();
        }
        else // Farbe von Text und Cursor je nach verbleibender Munition ändern
        {
            if (player.munition == 0) {textField.color = new Color(1, 0, 0, 1); mouseColor = new Color(1, 1, 1, 0.3f);}
            else if (player.munition <= 5) {textField.color = new Color(1, 0.92f, 0.016f, 1); mouseColor = new Color(1, 1, 1, 1);}
            else {textField.color = new Color(1, 1, 1, 1); mouseColor = new Color(1, 1, 1, 1);}
            cursorImage.color = mouseColor;
        }

        
    }
}
