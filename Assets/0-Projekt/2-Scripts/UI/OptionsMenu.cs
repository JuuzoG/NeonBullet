using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    private bool isFullScreen = true;
    public GameObject[] volumeObject;
    void Start()
    {
    }

    void Update()
    {
    }
    public void Toggle_FullScren()
    {
        if (isFullScreen) Screen.fullScreen = false;
        else Screen.fullScreen = true;
    }   

    public void Volume_Image(float volume)
    {
        if (volume == -80)
        {
            volumeObject[0].SetActive(false);
            volumeObject[1].SetActive(false);
            volumeObject[2].SetActive(false);
        } 
        if (volume > -80)
        {
            volumeObject[0].SetActive(true);
            volumeObject[1].SetActive(false);
            volumeObject[2].SetActive(false);
        }
        if (volume >= -40)
        {
            volumeObject[0].SetActive(true);
            volumeObject[1].SetActive(true);
            volumeObject[2].SetActive(false);
        }
        if (volume >= -10)
        {
            volumeObject[0].SetActive(true);
            volumeObject[1].SetActive(true);
            volumeObject[2].SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
    }

    public void Quit()
    {
    }
}
