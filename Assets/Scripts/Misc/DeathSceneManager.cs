using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSceneManager : MonoBehaviour
{
    [SerializeField] private float timeDelay = 0.0f;
    [SerializeField] private AnimationClip cutsceneClip;
    [SerializeField] private UIElementGroup deathEndGroup;
    [SerializeField] private UIElementGroup playAgainUI;
    [SerializeField] private GameObject image;

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


    private void Start() => StartCoroutine(DelayAnimation());

    private IEnumerator DelayAnimation()
    {
        GameManager.instance.FadeOut(timeDelay);
        yield return new WaitForSeconds(timeDelay + 1f);
        animator.SetTrigger("Cutscene");
        image.SetActive(false);
        yield return new WaitForSeconds(cutsceneClip.length);
        deathEndGroup.UpdateElements(0, 0, true);
    }


    public void PlayAgain()
    {
        deathEndGroup.UpdateElements(0, 0, false);
        playAgainUI.UpdateElements(0, 0, true);
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

        playAgainUI.UpdateElements(0, 0, false);

        yield return new WaitForSeconds(1.5f);
        if (!restart)
            GameManager.instance.LoadLevel(GameManager.instance.currentLevel);
        else
            GameManager.instance.LoadMainMenu();

    }
}
