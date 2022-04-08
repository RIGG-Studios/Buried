using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public float tutorialStayDuration;
    public float fadeInDuration;
    public Transform game;
    public string tutorialGroupTag;

    bool currentlyDisplaying;
    CanvasManager tutorialCanvas;
    UIElementGroup tutorialGroup;

    void Start()
    {
        tutorialCanvas = game.GetComponentInChildren<CanvasManager>();
        tutorialGroup = tutorialCanvas.FindElementGroupByID(tutorialGroupTag);
    }

    IEnumerator DisplayTutorial()
    {
        if (!currentlyDisplaying)
        {
            currentlyDisplaying = true;

            tutorialGroup.UpdateElements(1, fadeInDuration, true);

            yield return new WaitForSeconds(tutorialStayDuration);

            tutorialGroup.UpdateElements(0, fadeInDuration, false);

            currentlyDisplaying = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckCollision(collision))
        {
            StartCoroutine("DisplayTutorial");
        }
    }

    private bool CheckCollision(Collider2D collision)
    {
        Player player;
        collision.TryGetComponent(out player);

        return player != null;
    }
}
