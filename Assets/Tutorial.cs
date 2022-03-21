using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;

    [SerializeField] private List<TutorialContents> tutorialPages = new List<TutorialContents>();
    private int currentTutIndex = 0;

    private UIElementGroup tutorialGroup = null;
    private UIElement tutorialHeader = null;
    private UIElement tutorialContents = null;
    private PlayerCamera playerCamera;

    private void Awake()
    {
        instance = this;
        playerCamera = FindObjectOfType<PlayerCamera>();
        tutorialGroup = CanvasManager.instance.FindElementGroupByID("Tutorial");

        if (tutorialGroup != null)
        {
            tutorialHeader = tutorialGroup.FindElement("header");
            tutorialContents = tutorialGroup.FindElement("contents");
        }
    }

    public void StartTutorial()
    {
        tutorialGroup.UpdateElements(0, 0, true);
        UpdateTutorialPage(0);
    }

    private void EndTutorial()
    {
        tutorialGroup.UpdateElements(0, 0, false);
        Game.instance.UpdateState(GameStates.Playing);
    }

    public void UpdateTutorialPage(int next)
    {
        currentTutIndex += next;

        if (currentTutIndex > tutorialPages.Count - 1)
        {
            EndTutorial();
            return;
        }          
        else if (currentTutIndex < 0)
            currentTutIndex = tutorialPages.Count - 1;

        TutorialContents content = tutorialPages[currentTutIndex];

        if(content != null)
        {
            playerCamera.SetTarget(content.target);

            tutorialContents.OverrideValue(content.contents);
            tutorialHeader.OverrideValue(content.header);
        }
    }
}

[System.Serializable]
public class TutorialContents
{
    public Transform target;
    public string header;
    [TextArea]
    public string contents;
}
