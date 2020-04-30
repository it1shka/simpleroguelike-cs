using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }

    public void Retry()
    {
        Time.timeScale = 1;
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Menu()
    {
        Time.timeScale = 1;
        Application.LoadLevel("Menu");
    }

    public void Play()
    {
        Time.timeScale = 1;
        Application.LoadLevel("SampleScene");
    }

    public void Github()
    {
        Application.OpenURL("https://github.com/it1shka");
    }
}
