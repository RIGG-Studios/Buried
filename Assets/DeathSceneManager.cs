using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSceneManager : MonoBehaviour
{
    [SerializeField] private float timeDelay = 0.0f;
    [SerializeField] private AnimationClip cutsceneClip;

    private Animator animator
    {
        get
        {
            return GetComponent<Animator>();
        }
    }

    private CanvasManager canvasManager
    {
        get
        {
            return FindObjectOfType<CanvasManager>();
        }
    }

    private void Awake() => Fade(1f, 0f);

    private void Start() => StartCoroutine(DelayAnimation());

    private IEnumerator DelayAnimation()
    {
        Fade(0f, timeDelay);
        yield return new WaitForSeconds(timeDelay);
        animator.SetTrigger("Cutscene");
        yield return new WaitForSeconds(cutsceneClip.length);
        canvasManager.FindElementGroupByID("FadeGroup").FindElement("image").SetActive(false);
        canvasManager.ShowElementGroup(canvasManager.FindElementGroupByID("DeathEndGroup"), true);
    }

    public void Fade(float targetAlpha, float time)
    {
        canvasManager.FindElementGroupByID("FadeGroup").UpdateElements(targetAlpha, time, true);
    }

    public void PlayAgain()
    {
        canvasManager.ShowElementGroup(canvasManager.FindElementGroupByID("PlayAgainQuestionGroup"), true);
    }

    public void ExitToMainMenu()
    {
    }

    public void PlaySameLevel() => StartCoroutine(GoToNextLevel(false));
    

    public void RestartGame() => StartCoroutine(GoToNextLevel(true));

    private IEnumerator GoToNextLevel(bool restart)
    {
        if (GameManager.instance == null)
            yield break;

        canvasManager.ShowElementGroup(canvasManager.FindElementGroupByID("PlayAgainQuestionGroup"), false);
        Fade(1f, 1f);
        yield return new WaitForSeconds(1.5f);
        GameManager.instance.LoadNextLevelScene(restart ? -1 : 0);
    }
}
