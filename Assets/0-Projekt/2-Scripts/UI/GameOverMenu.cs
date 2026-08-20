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
            // Reload the player's last save exactly - position, health, ammo,
            // inventory and collected pickups all get reapplied.
            SaveManager.instance.Load(SaveManager.instance.currentSlot);
        }
        else
        {
            // Fallback: nothing has been saved yet this session, just reset the scene.
            Debug.LogWarning("GameOverMenu: No current save slot set, reloading scene from scratch instead.");
            SceneManager.LoadScene("Final");
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