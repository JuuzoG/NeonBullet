using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void retryButton()
    {
        if (GameManager.instance.state == GameStates.inGame) return;
        else GameManager.instance.state = GameStates.inGame;
        Time.timeScale = 1;
        SceneManager.LoadScene("Final"); 
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
