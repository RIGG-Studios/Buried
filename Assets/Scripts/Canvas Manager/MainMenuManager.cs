using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField, Range(0, 10f)] private float playGameDelay = 0.0f;

    CanvasManager manager;
    bool toggledOptions;

    private void Start()
    {
        manager = GetComponent<CanvasManager>();
        manager.FindElementGroupByID("FadeGroup").FindElement("image").SetActive(false);
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

    public void PlayGame() => StartCoroutine(IEPlayGame());


    private IEnumerator IEPlayGame()
    {
        manager.FindElementGroupByID("FadeGroup").FindElement("image").SetActive(true);
        manager.FindElementGroupByID("FadeGroup").UpdateElements(1f, playGameDelay, true);
            
        yield return new WaitForSeconds(playGameDelay + 1f);
        GameManager.instance.LoadNextLevelScene(0);
    }
}
