using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    CanvasManager manager;
    bool toggledOptions;

    private void Start()
    {
        manager = GetComponent<CanvasManager>();
    }

    public void DisplayCredits() => Debug.Log("this is a work in progress uWu");

    public void ToggleOptions()
    {
        UIElementGroup optionsGroup = manager.FindElementGroupByID("OptionsGroup");

        if (toggledOptions)
        {
            optionsGroup.UpdateElements(0, 0.5f, false);
        }
        else
        {
            optionsGroup.UpdateElements(0, 0.5f, true);
        }

        toggledOptions = !toggledOptions;
    }

    public void QuitGame() => Application.Quit();

    public void PlayGame() => SceneManager.LoadScene("Level1");
}
