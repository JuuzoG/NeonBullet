using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseScreen;
    public GameObject OptionScreen;
    private Player player;
    bool isPaused;
    bool isInSettings;
    void Start()
    {
        GameObject playerGEt = GameObject.FindGameObjectWithTag("Player");
        player = playerGEt.GetComponent<Player>();
        OptionScreen.SetActive(false);
        PauseScreen.SetActive(false);
    }

    void Update()
    {
        if (GameManager.instance.state == GameStates.inventory)
            return;
        if (Input.GetKeyDown(player.Menu)) Pause_Resume();
    }
    public void Pause_Resume()
    {

    if (isPaused)
    {
        isPaused = false;
        GameManager.instance.state = GameStates.inGame;
        Time.timeScale = 1;
        PauseScreen.SetActive(false);
    }
    else
    {
        isPaused = true;
        GameManager.instance.state = GameStates.paused;
        Time.timeScale = 0;
        PauseScreen.SetActive(true);
    }
        
        
    }   

    public void Options_Retrun()
    {
        if (isInSettings)
        {
            isInSettings = false;
            OptionScreen.SetActive(false);
        }
        else
        {
            isInSettings = true;
            OptionScreen.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu SaveSystem");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
