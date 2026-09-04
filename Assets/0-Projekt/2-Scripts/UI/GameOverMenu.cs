using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void retryButton()
    {
        Time.timeScale = 1;
        GameManager.instance.state = GameStates.inGame;

        if (SaveManager.instance != null && SaveManager.instance.HasCurrentSlot)
        {
            SaveManager.instance.Load(SaveManager.instance.currentSlot);
        }
        else
        {
            SceneManager.LoadScene("JasminLvl1 Gasse");
        }
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