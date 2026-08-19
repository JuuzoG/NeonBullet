using System.Diagnostics;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject devMenu;

    string scene = "Final";
    private bool isDev = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) Dev();
    }

    public void startButton()
    {
        Time.timeScale = 1;
        GameManager.instance.state = GameStates.inGame;
        SceneManager.LoadScene(scene);
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
        UnityEngine.Debug.Log("Exit was called");
        Application.Quit();
        //EditorApplication.ExitPlaymode(0);
    }

    void Dev()
    {
        isDev = !isDev;
        devMenu.SetActive(isDev);
        
    }

    public void DevInput(string input)
    {
        scene = input;
    }

    public void DevDropdown(int option)
    {
        switch (option)
        {
            case 0:
                scene = "Final";
                break;
            case 1:
                scene = "JasminLvl1Gasse";
                break;
            case 2:
                scene = "juuzbackup";
                break;
            case 3:
                scene = "Lennard";
                break;
            case 4:
                scene = "Marcel";
                break;
            case 5:
                scene = "mewo";
                break;
            case 6:
                scene = "ubeyd";
                break;
            case 7:
                scene = "BulletTrail";
                break;
        }
    }

    public void LevelDropdown(int levelselect)
    {
        switch (levelselect)
        {
            case 0:
                scene = "Final";
                break;
        }
    }
}
