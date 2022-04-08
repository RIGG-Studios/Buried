using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Image levelIcon = null;
    [SerializeField] private Text levelName = null;
    [SerializeField] private Text levelDescription = null;
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
            levelIcon.CrossFadeAlpha(1f, 0, true);
            levelName.CrossFadeAlpha(1f, 0, true);
            levelDescription.CrossFadeAlpha(1f, 0, true);

            levelIcon.sprite = levelProperties.properties.levelIcon;
            levelName.text = levelProperties.properties.levelName + " | " + levelProperties.properties.levelDifficulty;
            levelDescription.text = levelProperties.properties.levelDescription;
            button.interactable = true;
        }
        else
        {
            levelIcon.CrossFadeAlpha(lockedAlpha, 0, true);
            levelName.CrossFadeAlpha(lockedAlpha, 0, true);
            levelDescription.CrossFadeAlpha(lockedAlpha, 0, true);

            levelIcon.sprite = levelProperties.properties.levelIcon;
            levelName.text = "LEVEL LOCKED!";
            levelDescription.text = "LEVEL LOCKED!";
            button.interactable = false;
        }
    }

    public void EnterLevel()
    {
        if (levelProperties == null || !levelProperties.unlocked)
            return;
        
        mainMenu.EnterLevel(levelProperties);
    }
}
