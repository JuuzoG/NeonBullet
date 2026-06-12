using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void retryButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void mainMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    
    public void exitButton()
    {
        Application.Quit();
    }
}
