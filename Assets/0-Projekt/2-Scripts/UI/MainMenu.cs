using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject lastSelectedElement;

    public void startButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void optionsButton()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void creditsButton()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void returnButton()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }
    
    public void exitButton()
    {
        Application.Quit();
    }
    private void Reset()
    {
        eventSystem = Object.FindFirstObjectByType<EventSystem>();

        if (!eventSystem)
        {
            UnityEngine.Debug.Log("Did not find an Event System in this scene.", this);
            return;
        }

        lastSelectedElement = eventSystem.firstSelectedGameObject;
    }

    private void Update()
    {
        if (!eventSystem)
            return;

        if (eventSystem.currentSelectedGameObject &&
            lastSelectedElement != eventSystem.currentSelectedGameObject)
            lastSelectedElement = eventSystem.currentSelectedGameObject;

        if (!eventSystem.currentSelectedGameObject && lastSelectedElement)
            eventSystem.SetSelectedGameObject(lastSelectedElement);
    }
}





public class EventSystemAccess : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Selectable firstItemToSelect;

    private void Start()
    {
        if (eventSystem == null)
            return;

        eventSystem.firstSelectedGameObject = firstItemToSelect.gameObject;
    }
}
