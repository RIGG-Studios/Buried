using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused { get; private set; }

    public bool optionsOpened { get; private set; }

    public static PauseMenu instance;

    private UIElementGroup pauseGroup = null;
    private UIElementGroup optionsGroup = null;

    private Player player
    {
        get
        {
            return FindObjectOfType<Player>();
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        pauseGroup = CanvasManager.instance.FindElementGroupByID("PauseGroup");
        optionsGroup = CanvasManager.instance.FindElementGroupByID("OptionsGroup");
    }

    public void PauseGame()
    {
        if (optionsOpened)
        {
            optionsGroup.UpdateElements(0, 0, false);
            optionsOpened = false;
            return;
        }

        if (isPaused)
        {
            ResumeGame();
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

    }
}
