using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    public string menuSceneName = "Menu";

    void Start()
    {
        if (SceneTransitionManager.instance != null)
            SceneTransitionManager.instance.FadeAndLoad(menuSceneName);
        else
            SceneManager.LoadScene(menuSceneName);
    }
}