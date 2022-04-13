using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Text levelName = null;
    [SerializeField] private Text levelDifficulty = null;
    [SerializeField] private Text levelTimeEstimate = null;

    [SerializeField] private float lockedAlpha;

    private Level levelProperties = null;
    private MainMenuManager mainMenu = null;
    private Button button = null;

    public void Initialize(Level levelProperties)
    {
        mainMenu = FindObjectOfType<MainMenuManager>();
        button = GetComponent<Button>();
        this.levelProperties = levelProperties;

        if (levelProperties.unlocked)
        {
            LerpAlphas(1f);
            levelName.text = levelProperties.properties.levelName;
            levelDifficulty.text = "DIFFICULTY: " + levelProperties.properties.levelDifficulty;
            levelTimeEstimate.text = "TIME ESTIMATE: " + levelProperties.properties.timeEstimate;
            button.interactable = true;
        }
        else
        {
            LerpAlphas(lockedAlpha);
            levelName.text = "LEVEL LOCKED!";
            levelDifficulty.text = "LOCKED!";
            levelTimeEstimate.text = "LOCKED!";
            button.interactable = false;
        }
    }

    private void OnEnable()
    {
        if (levelProperties.unlocked)
            return;

        LerpAlphas(lockedAlpha);
    }

    public void EnterLevel()
    {
        if (levelProperties == null || !levelProperties.unlocked)
            return;

        mainMenu.EnterLevel(levelProperties);
    }

    private void LerpAlphas(float target)
    {
        levelName.CrossFadeAlpha(target, 0, true);
        levelDifficulty.CrossFadeAlpha(target, 0, true);
        levelTimeEstimate.CrossFadeAlpha(target, 0, true);
    }
}
