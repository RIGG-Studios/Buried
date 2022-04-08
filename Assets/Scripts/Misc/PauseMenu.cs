using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused { get; private set; }
    public bool optionsOpened { get; private set; }
    public bool exitOpened { get; private set; }

    private UIElementGroup pauseGroup = null;
    private UIElementGroup optionsGroup = null;
    private UIElementGroup exitGroup = null;
    private Player player = null;

    private void Start()
    {
        player = FindObjectOfType<Player>();

        pauseGroup = player.playerCanvas.FindElementGroupByID("PauseGroup");
        optionsGroup = player.playerCanvas.FindElementGroupByID("OptionsGroup");
        exitGroup = player.playerCanvas.FindElementGroupByID("ExitGameGroup");
    }

    public void PauseGame()
    {
        if (optionsOpened)
        {
            player.playerCanvas.HideElementGroup(optionsGroup);
            optionsOpened = false;
            return;
        }

        if (isPaused)
        {
            ResumeGame();
            return;
        }

        if (exitOpened)
        {
            player.playerCanvas.HideElementGroup(exitGroup);
            exitOpened = false;
            Time.timeScale = 1f;
            return;
        }

        pauseGroup.UpdateElements(0, 0, true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if(optionsOpened)
            optionsGroup.UpdateElements(0, 0, false);

        pauseGroup.UpdateElements(0, 0, false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ToggleOptions()
    {
        optionsGroup.UpdateElements(0, 0, true);
        optionsOpened = true;
    }


    public void ExitGame()
    {
        PauseGame();
        player.playerCanvas.ShowElementGroup(exitGroup, true);
        Time.timeScale = 0f;
        exitOpened = true;
    }

    public void Exit(bool desktop)
    {
        if (desktop)
            GameManager.instance.ExitGame();
        else
            GameManager.instance.LoadMainMenu();

        Time.timeScale = 1f;
    }

}
