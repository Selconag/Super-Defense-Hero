using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenuPanel, OptionsMenuPanel, PauseMenuPanel;
    public GameObject GUIPanel;
    private bool optionsOpen;

    public static Action GameStarted;
    public static MenuManager Instance;

    private void Awake() => Instance = this;

    public void StartGame()
    {
        MainMenuPanel.SetActive(false);
        GUIPanel.SetActive(true);
        Debug.Log("Game Started");
        GameStarted.Invoke();
    }

    public void ToggleOptions()
    { 
        optionsOpen =! optionsOpen;
        OptionsMenuPanel.SetActive(optionsOpen);
        Debug.Log("Options");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

public static class BooleanExtensions
{
    public static int ToInt(this bool value)
    {
        return value ? 1 : 0;
    }
}
