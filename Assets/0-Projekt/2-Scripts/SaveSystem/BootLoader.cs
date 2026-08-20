using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    public string menuSceneName = "Menu";

    void Start()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}