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
        if (Input.GetKeyDown(player.Menu))Pause_Resume();
    }
    public void Pause_Resume()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1;
            PauseScreen.SetActive(false);
        }
        else
        {
            isPaused = true;
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
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
