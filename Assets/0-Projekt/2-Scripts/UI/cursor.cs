using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
//von AI | bedeute das AI es erklärte hat.
public class Cursor : MonoBehaviour
{
    
    [SerializeField] private InputActionReference pointerPositionAction; //InputSystem UI/Point
    [SerializeField] private TMP_Text textField; //Amo count
    [SerializeField] private GameObject[] cursorObj; //Object with Animation.cs, Image(Cursor) and Animator
    [SerializeField] private GameObject idleRailgun;

    [Header("private")]
    private RectTransform _cursorTransform; // die position des Cursor Object
    private Canvas _parentCanvas; // Canvas, in dem sich der Cursor befindet
    private RectTransform _canvasRectTransform; // RectTransform des Canvas, für Koordinatenumrechnung | von AI
    private Camera _canvasCamera; // Die Kamera
    [Header("")]
    private Image[] cursorImage;
    private Animations[] cursorAnim;
    private Color mouseColor;
    [Header("Scripts")]
    private Player player;
    private WeaponSelector selectedWeapon;

    private void Start()
    {
        player = GameManager.instance.player;
        selectedWeapon = GameManager.instance.WeaponSelect;
        idleRailgun.SetActive(false);

        cursorImage = new Image[cursorObj.Length];
        cursorAnim = new Animations[cursorObj.Length];
        for (int i = 0; i < cursorObj.Length; i++)
        {
            cursorImage[i] = cursorObj[i].GetComponent<Image>();
            cursorAnim[i] = cursorObj[i].GetComponent<Animations>();
        }
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
        if (GameManager.instance.state == GameStates.inGame && selectedWeapon.CurrentWeaponIndex == 0 || selectedWeapon.CurrentWeaponIndex == 2) // Munitionsanzeige nur während des Spiels anzeigen, sonst leer lassen
        textField.text = "" + player.munition;
        else 
        textField.text = "";

        if (selectedWeapon.CurrentWeaponIndex == 2)
        {
            textField.rectTransform.anchoredPosition3D = new Vector3(112.15f,43.74f,0f);
        }
        else textField.rectTransform.anchoredPosition3D = new Vector3(75.3f, 65.1f, 0f);

        if(selectedWeapon.CurrentWeaponIndex == 1) idleRailgun.SetActive(true);
        else idleRailgun.SetActive(false);

        if (player.munition == 0) {textField.color = new Color(1, 0, 0, 1); mouseColor = new Color(1, 1, 1, 0.3f);}
        else if (player.munition <= 5) {textField.color = new Color(1, 0.92f, 0.016f, 1); mouseColor = new Color(1, 1, 1, 1);}
        else {textField.color = new Color(1, 1, 1, 1); mouseColor = new Color(1, 1, 1, 1);}
        cursorImage[selectedWeapon.CurrentWeaponIndex].color = mouseColor;

        switch (selectedWeapon.CurrentWeaponIndex)
            {
                case 0:
                    cursorObj[0].SetActive(true);
                    cursorObj[1].SetActive(false);
                    cursorObj[2].SetActive(false);
                    break;
                case 1:
                    cursorObj[0].SetActive(false);
                    cursorObj[1].SetActive(true);
                    cursorObj[2].SetActive(false);
                    break;
                case 2:
                    cursorObj[0].SetActive(false);
                    cursorObj[1].SetActive(false);
                    cursorObj[2].SetActive(true);
                    break;
                default:
                    Debug.Log("Oh shit",this);
                    break;
            }    
    }
}
